using System;
using System.Text;

namespace Macquarie.Handbook.WebApi
{
    public abstract class HandbookApiRequestBuilder
    {
        protected StringBuilder API_STRING = new StringBuilder(250);

        private string _code = "NO_CODE";
        public string Code
        {
            get { return _code; }
            set { 
                if (value != null)
                    _code = value;                             
            }
        }
        
        
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