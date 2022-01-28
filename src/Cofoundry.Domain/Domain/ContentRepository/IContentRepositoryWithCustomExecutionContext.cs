﻿using Cofoundry.Domain.CQS;

namespace Cofoundry.Domain
{
    /// <summary>
    /// An <see cref="IContentRepository"/> that allows a specific
    /// execution context to be set and used for any chained queries 
    /// or commands.
    /// </summary>
    public interface IContentRepositoryWithCustomExecutionContext
        : IContentRepository
        , IAdvancedContentRepository
    {
        /// <summary>
        /// Sets the execution context for any queries or commands
        /// chained of this instance. Typically used to impersonate
        /// a user or elevate permissions.
        /// </summary>
        /// <param name="executionContext">
        /// The execution context instance to use. May pass null to reset 
        /// this instance and use the default.
        /// </param>
        void SetExecutionContext(IExecutionContext executionContext);
    }
}
