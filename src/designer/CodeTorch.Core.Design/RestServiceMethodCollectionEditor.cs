using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace CodeTorch.Core.Design
{
    public class RestServiceMethodCollectionEditor : CollectionEditor
    {
        public RestServiceMethodCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type[] CreateNewItemTypes()
        {
            List<Type> types = new List<Type>();

            types.Add(typeof(GetRestServiceMethod));
            types.Add(typeof(PostRestServiceMethod));
            types.Add(typeof(PutRestServiceMethod));
            types.Add(typeof(DeleteRestServiceMethod));

            return types.ToArray<Type>();
        }

        protected override string GetDisplayText(object value)
        {
            BaseRestServiceMethod method = (BaseRestServiceMethod)value;

            string retVal = String.Empty;
            retVal = String.Format("{0} - {1}",method.Action, method.RequestDataCommand);

            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {

            object retVal = base.CreateInstance(itemType);

            switch (itemType.Name.ToLower())
            { 
                case "postrestservicemethod":
                    ((BaseRestServiceMethod)retVal).Action = RestServiceMethodActionEnum.POST;
                    break;
                case "putrestservicemethod":
                    ((BaseRestServiceMethod)retVal).Action = RestServiceMethodActionEnum.PUT;
                    break;
                case "deleterestservicemethod":
                    ((BaseRestServiceMethod)retVal).Action = RestServiceMethodActionEnum.DELETE;
                    break;
                default:
                    ((BaseRestServiceMethod)retVal).Action = RestServiceMethodActionEnum.GET;
                    break;
            }

            if(Context.Instance != null)
            {
                BaseRestServiceMethod method = (BaseRestServiceMethod)retVal;
                method.ParentService = Context.Instance as RestService;
            }
   
            return retVal;
        }
    }
}
