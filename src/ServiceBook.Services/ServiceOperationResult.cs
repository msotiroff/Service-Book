using ServiceBook.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ServiceBook.Services
{
    public class ServiceOperationResult : IServiceOperationResult
    {
        public ServiceOperationResult(params string[] errors)
        {
            this.Errors = errors.ToList();
        }

        public static ServiceOperationResult Succeeded => new ServiceOperationResult();
        
        public bool Success => this.Errors.Count == 0;

        public IList<string> Errors { get; } = new List<string>();

        public string ReferenceId { get; private set; }

        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        public void SetReferenceId(string reference)
        {
            this.ReferenceId = reference;
        }

        public override string ToString()
        {
            return string.Join(", ", this.Errors);
        }
    }
}
