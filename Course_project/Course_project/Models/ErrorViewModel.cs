using System;

namespace Course_project.Models
{
    /// <summary>
    /// ErrorViewModel class
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
