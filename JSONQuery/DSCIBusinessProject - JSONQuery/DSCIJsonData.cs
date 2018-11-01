using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSCIBusinessProject___JSONQuery
{
    class DSCIJsonData
    {
        #region JSON Serialized Data

        public string from { get; set; }
        public string authenticated_with { get; set; }
        public string search_term { get; set; }
        public string current_result_filter { get; set; }
        public string organization_id { get; set; }
        public string organization_name { get; set; }
        public string organization { get; set; }
        public string origin { get; set; }
        public string post_id { get; set; }
        public string post_name { get; set; }
        public string created_by_id { get; set; }
        public string created_by_name { get; set; }
        public string owned_by_id { get; set; }
        public string owned_by_name { get; set; }
        public string category { get; set; }
        public string url { get; set; }
        public string response_status { get; set; }
        public string request_url { get; set; }
        public string request_method { get; set; }
        public string response_message { get; set; }

        #endregion // JSON Serialized data

        public Dictionary<string, string> Json { get; set; }

        #region Base Row Items

        public string EventId { get; set; }
        public string Event { get; set; }
        public string Occurrence { get; set; }

        #endregion // Base Row Items

        public static List<string> ColumnNames = new List<string>()
        {
            "EventId"
            , "Event"
            , "Occurrence"
            , "from"
            , "authenticated_with"
            , "search_term"
            , "current_result_filter"
            , "organization_id"
            , "organization_name"
            , "organization"
            , "origin"
            , "post_id"
            , "post_name"
            , "created_by_id"
            , "created_by_name"
            , "owned_by_id"
            , "owned_by_name"
            , "category"
            , "url"
            , "response_status"
            , "request_url"
            , "request_method"
            , "response_message"
        };
    }
}
