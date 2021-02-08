using System;
using System.Text;

namespace Macquarie.Handbook.WebApi
{
    public static class HandbookApiRequest
    {
        private static readonly ApiRequest UNIT_API_REQUEST = new ApiRequest("https://coursehandbook.mq.edu.au/api/content/render/false/query/+contentType:mq2_p", "subject");
        private static readonly ApiRequest COURSE_API_REQUEST = new ApiRequest("https://macquarie-prod-handbook.factor5-curriculum.com.au/api/content/render/false/query/+contentType:mq2_p", "course");

        public static ApiRequest UnitRequest {
            get { return UNIT_API_REQUEST; }
        }

        public static ApiRequest CourseRequest {
            get { return COURSE_API_REQUEST; }
        }
    }

    public class ApiRequest {
        protected readonly string apiStringBase;
        protected readonly string requestObjectString;
        private StringBuilder apiRequest;
        public string BaseRequest { get { return apiStringBase; } }

        public ApiRequest(string baseApiUrl, string objectString) {
            apiStringBase = baseApiUrl;
            requestObjectString = objectString;
        }

        private ApiRequest AppendRequestFormatInternal(string filter, params object[] values) {
            this.apiRequest.AppendFormat(filter, values);
            return this;
        }

        private ApiRequest AppendRequestInternal(string value) {
            this.apiRequest.Append(value);
            return this;
        }

        private ApiRequest AppendFilterInternal(string filter, object value) {
            return AppendRequestFormatInternal("%20+mq2_p{0}.{1}:{2}", requestObjectString, filter, value);
        }

        public ApiRequest SetLimit(int limit) {
            return AppendRequestFormatInternal("/limit/{0}", limit);
        }

        public ApiRequest FilterImplementationYear(int year) {
            return AppendFilterInternal("implementationYear", year);
        }

        public ApiRequest FilterCode(string code) {
            return AppendFilterInternal("code", code);
        }

        public ApiRequest FilterParentAcademicOrganisation(string academicOrganisationHash) {
            return AppendFilterInternal("parentAcademicOrg", academicOrganisationHash);
        }

        public ApiRequest FilterStudyLevel(int studylevel) {
            return AppendFilterInternal("studyLevel", studylevel);
        }

        public string GetRequest() {
            return apiRequest.ToString();
        }
    }

    

    public abstract class HandbookApiRequestBuilder
    {
        protected StringBuilder API_STRING = new StringBuilder(250);
        public string Code { get; set; } = "NO_CODE";
        public int ImplementationYear { get; set; } = -1;
        public int Limit { get; set; } = -1;

        public abstract override string ToString();
        public abstract void Reset();
    }
    
    public class UnitApiRequestBuilder : HandbookApiRequestBuilder
    {
        private const string BASE_API_STRING = "https://coursehandbook.mq.edu.au/api/content/render/false/query/+contentType:mq2_psubject";

        public override void Reset()
        {
            Code = "NO_CODE";
            ImplementationYear = -1;
            Limit = -1;
        }

        public override string ToString()
        {               
            API_STRING.Clear();
            API_STRING.Append(BASE_API_STRING);

            if (ImplementationYear != -1) {
                API_STRING.AppendFormat("%20+mq2_psubject.implementationYear:{0}", ImplementationYear);
            }
            if (Code != "NO_CODE") {
                API_STRING.AppendFormat("%20+mq2_psubject.code:{0}", Code);
            }
            if (Limit != -1) {
                API_STRING.AppendFormat("/limit/{0}", Limit);
            }
            return API_STRING.ToString();
        }
    }

    public class CourseApiRequestBuilder : HandbookApiRequestBuilder
    {
        private const string BASE_API_STRING = "https://macquarie-prod-handbook.factor5-curriculum.com.au/api/content/render/false/query/+contentType:mq2_pcourse";

        public override void Reset()
        {
            Code = "NO_CODE";
            ImplementationYear = -1;
            Limit = -1;
        }

        public override string ToString()
        {                    
            API_STRING.Clear();
            API_STRING.Append(BASE_API_STRING);

            if (ImplementationYear != -1) {
                API_STRING.AppendFormat("%20+mq2_pcourse.implementationYear:{0}", ImplementationYear);
            }
            if (Code != "NO_CODE") {
                API_STRING.AppendFormat("%20+mq2_pcourse.code:{0}", Code);
            }
            if (Limit != -1) {
                API_STRING.AppendFormat("/limit/{0}", Limit);
            }
            return API_STRING.ToString();
        }
    }
}