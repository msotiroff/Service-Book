using System.Collections.Generic;

namespace ServiceBook.Services.Interfaces
{
    public interface IServiceOperationResult
    {
        bool Success { get; }

        string ReferenceId { get; }

        IList<string> Errors { get; }

        void AddError(string error);

        void SetReferenceId(string reference);
    }
}
