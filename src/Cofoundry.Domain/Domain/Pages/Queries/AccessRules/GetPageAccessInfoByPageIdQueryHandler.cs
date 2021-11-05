﻿using Cofoundry.Domain.CQS;
using Cofoundry.Domain.Data;
using Cofoundry.Domain.Internal;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cofoundry.Domain
{
    /// <summary>
    /// Returns all access rules associated with a page, including those inherited from
    /// parent directories.
    /// </summary>
    public class GetPageAccessInfoByPageIdQueryHandler
        : IQueryHandler<GetPageAccessInfoByPageIdQuery, PageAccessInfo>
        , IPermissionRestrictedQueryHandler<GetPageAccessInfoByPageIdQuery, PageAccessInfo>
    {
        private readonly CofoundryDbContext _dbContext;
        private readonly IEntityAccessInfoMapper _entityAccessInfoMapper;
        private readonly IPageDirectoryMicroSummaryMapper _pageDirectoryMicroSummaryMapper;

        public GetPageAccessInfoByPageIdQueryHandler(
            CofoundryDbContext dbContext,
            IEntityAccessInfoMapper entityAccessInfoMapper,
            IPageDirectoryMicroSummaryMapper pageDirectoryMicroSummaryMapper
            )
        {
            _dbContext = dbContext;
            _entityAccessInfoMapper = entityAccessInfoMapper;
            _pageDirectoryMicroSummaryMapper = pageDirectoryMicroSummaryMapper;
        }

        public async Task<PageAccessInfo> ExecuteAsync(GetPageAccessInfoByPageIdQuery query, IExecutionContext executionContext)
        {
            var dbPage = await _dbContext
                .Pages
                .AsNoTracking()
                .Include(p => p.AccessRules)
                .FilterActive()
                .FilterById(query.PageId)
                .SingleOrDefaultAsync();

            if (dbPage == null) return null;

            var result = new PageAccessInfo();
            await _entityAccessInfoMapper.MapAsync(dbPage, result, executionContext, (dbRule, rule) =>
            {
                rule.PageId = dbRule.PageId;
                rule.PageAccessRuleId = dbRule.PageAccessRuleId;
            });

            result.PageId = dbPage.PageId;
            await MapInheritedRules(dbPage, result, executionContext);

            return result;
        }

        private async Task MapInheritedRules(Page dbPage, PageAccessInfo result, IExecutionContext executionContext)
        {
            var dbInheritedRules = await _dbContext
                .PageDirectoryClosures
                .AsNoTracking()
                .Include(d => d.AncestorPageDirectory)
                .ThenInclude(d => d.AccessRules)
                .Include(d => d.AncestorPageDirectory)
                .ThenInclude(d => d.PageDirectoryPath)
                .FilterByDescendantId(dbPage.PageDirectoryId)
                .Where(d => d.DescendantPageDirectoryId == dbPage.PageDirectoryId && d.AncestorPageDirectory.AccessRules.Any())
                .OrderByDescending(d => d.Distance)
                .ToListAsync();

            result.InheritedAccessRules = new List<InheritedPageDirectoryAccessInfo>();

            foreach (var dbInheritedRule in dbInheritedRules)
            {
                var inheritedDirectory = new InheritedPageDirectoryAccessInfo();
                inheritedDirectory.PageDirectory = _pageDirectoryMicroSummaryMapper.Map(dbInheritedRule.AncestorPageDirectory);
                await _entityAccessInfoMapper.MapAsync(dbInheritedRule.AncestorPageDirectory, inheritedDirectory, executionContext, (dbRule, rule) =>
                {
                    rule.PageDirectoryId = dbRule.PageDirectoryId;
                    rule.PageDirectoryAccessRuleId = dbRule.PageDirectoryAccessRuleId;
                });

                result.InheritedAccessRules.Add(inheritedDirectory);
            }
        }

        public IEnumerable<IPermissionApplication> GetPermissions(GetPageAccessInfoByPageIdQuery query)
        {
            yield return new PageReadPermission();
        }
    }
}
