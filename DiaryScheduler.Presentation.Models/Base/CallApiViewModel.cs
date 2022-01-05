namespace DiaryScheduler.Presentation.Models.Base
{
    /// <summary>
    /// The call api view model.
    /// </summary>
    public class CallApiViewModel
    {
        /// <summary>
        /// Gets or sets the api url.
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the grant type.
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the authentication api url.
        /// </summary>
        public string AuthApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the endpoint to call.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the query parameters.
        /// </summary>
        public object QueryParams { get; set; }

        /// <summary>
        /// Gets or sets the body of the request.
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the endpoint requires authentication to call.
        /// </summary>
        public bool RequiresAuthentication { get; set; }
    }
}
