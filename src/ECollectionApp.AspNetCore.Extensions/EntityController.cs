using ECollectionApp.AspNetCore.Microservice.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECollectionApp.AspNetCore.Microservice
{
    /// <summary> Controller containing most common functions operating on <see cref="DbContext"/> </summary>
    public abstract class EntityController<TEntity> : ControllerBase where TEntity : class
    {
        public EntityController(DbContext context, ILogger<EntityController<TEntity>> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger;
        }

        protected DbContext Context { get; }

        protected ILogger<EntityController<TEntity>> Logger { get; }

        protected abstract int GetEntityId(TEntity entity);

        protected abstract void SetEntityId(TEntity entity, int id);

        /// <summary> Get all entities existing in database </summary>
        protected async Task<ActionResult<IEnumerable<TEntity>>> GetEntities(bool authorizeResults = true)
        {
            TEntity[] result = await Context.Set<TEntity>().ToArrayAsync();
            if (authorizeResults)
            {
                foreach (TEntity entity in result)
                {
                    AuthorizationResult authResult = await this.AuthorizeAsync(entity, Operations.Read);
                    if (!authResult.Succeeded)
                    {
                        return Forbid();
                    }
                }
            }
            return result;
        }

        /// <summary> Get all entities existing in database matching query </summary>
        protected async Task<ActionResult<IEnumerable<TEntity>>> GetEntities(Func<IQueryable<TEntity>, IQueryable<TEntity>> query, bool authorizeResults = true)
        {
            TEntity[] result = await query.Invoke(Context.Set<TEntity>()).ToArrayAsync();
            if (authorizeResults)
            {
                foreach (TEntity entity in result)
                {
                    AuthorizationResult authResult = await this.AuthorizeAsync(entity, Operations.Read);
                    if (!authResult.Succeeded)
                    {
                        return Forbid();
                    }
                }
            }
            return result;
        }

        /// <summary> Get entity with given id </summary>
        protected async Task<ActionResult<TEntity>> GetEntity(int id, bool authorizeResults = true)
        {
            TEntity entity = await Context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                if (authorizeResults)
                {
                    return await this.AuthorizeResultAsync(entity, Operations.Read);
                }
                return entity;
            }
            return NotFound();
        }

        /// <summary> Add entity to database </summary>
        protected async Task<ActionResult<TEntity>> PostEntity(TEntity value, Func<CreatedAtActionResult> getResult, bool authorizeResults = true)
        {
            if (value == null || GetEntityId(value) != 0)
            {
                return BadRequest();
            }
            if (authorizeResults)
            {
                AuthorizationResult result = await this.AuthorizeAsync(value, Operations.Create);
                if (!result.Succeeded)
                {
                    return Forbid();
                }
            }
            await Context.Set<TEntity>().AddAsync(value);
            await Context.SaveChangesAsync();
            return getResult.Invoke();
        }

        /// <summary> Update entity in database </summary>
        protected async Task<IActionResult> PutEntity(int id, TEntity value, bool authorizeResults = true)
        {
            int entityId = GetEntityId(value);
            if (value == null || (entityId > 0 && entityId != id))
            {
                return BadRequest();
            }
            if (entityId == 0)
            {
                SetEntityId(value, id);
            }
            if (authorizeResults)
            {
                AuthorizationResult result = await this.AuthorizeAsync(value, Operations.Update);
                if (!result.Succeeded)
                {
                    return Forbid();
                }
            }
            Context.Entry(value).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                TEntity foundEntity = await Context.Set<TEntity>().FindAsync(id);
                if (foundEntity == null)
                {
                    return NotFound();
                }
                Logger.LogError(ex, $"Error occured while executing {nameof(PutEntity)}");
                throw;
            }
            return NoContent();
        }

        /// <summary> Remove entity from database </summary>
        protected async Task<IActionResult> DeleteEntity(bool authorizeResults, params object[] keyValues)
        {
            if (keyValues == null)
            {
                return BadRequest($"{nameof(keyValues)} is null");
            }
            TEntity entity = await Context.Set<TEntity>().FindAsync(keyValues);
            if (entity == null)
            {
                return NotFound();
            }
            if (authorizeResults)
            {
                AuthorizationResult result = await this.AuthorizeAsync(entity, Operations.Delete);
                if (!result.Succeeded)
                {
                    return Forbid();
                }
            }
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary> Remove entity from database </summary>
        protected Task<IActionResult> DeleteEntity(params object[] keyValues) => DeleteEntity(true, keyValues);

        /// <summary> Remove entity from database </summary>
        protected Task<IActionResult> DeleteEntity(int id, bool authorizeResults = true) => DeleteEntity(authorizeResults, new object[] { id });

        protected IQueryable<TEntity> PaginateEntitiesQuery(IQueryable<TEntity> entities, PaginationMetadata pagination)
        {
            if (pagination == null || pagination == PaginationMetadata.Empty)
            {
                return entities;
            }
            int count = entities.Count();
            PaginationCounter counter = new PaginationCounter(count, pagination.Page, pagination.Limit, 1);
            return entities.Skip(counter.StartIndex).Take(pagination.Limit);
        }

        protected IEnumerable<TEntity> PaginateEntities(IEnumerable<TEntity> entities, PaginationMetadata pagination)
        {
            if (pagination == null || pagination == PaginationMetadata.Empty)
            {
                return entities;
            }
            int count = entities.Count();
            PaginationCounter counter = new PaginationCounter(count, pagination.Page, pagination.Limit, 1);
            return entities.Skip(counter.StartIndex).Take(pagination.Limit);
        }

        protected StatusCodeResult InternalError() => StatusCode(500);

        protected ObjectResult InternalError(object value) => StatusCode(500, value);

        protected StatusCodeResult Gone() => StatusCode(410);

        public override ActionResult ValidationProblem() => ValidationProblem(ModelState);

        public override ActionResult ValidationProblem(
            string detail = null,
            string instance = null,
            int? statusCode = null,
            string title = null,
            string type = null,
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary = null)
            => ValidationProblem(modelStateDictionary);

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            IList<KeyValuePair<string, string[]>> errors = new List<KeyValuePair<string, string[]>>();
            foreach (KeyValuePair<string, ModelStateEntry> entry in modelStateDictionary)
            {
                errors.Add(new KeyValuePair<string, string[]>(entry.Key, entry.Value.Errors.Select(er => er.ErrorMessage).ToArray()));
            }
            return ValidationProblem(errors);
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ValidationProblemDetails descriptor)
            => ValidationProblem(descriptor.Errors.ToList());

        public ActionResult ValidationProblem(IEnumerable<KeyValuePair<string, string[]>> errors)
            => new JsonResult(errors) { StatusCode = 400 };

    }
}
