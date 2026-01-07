namespace Leoni.Utils.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class PermissionAttribute : Attribute
    {
        public string [] Permissions  {get;set;}
        public bool RequiredAll { get;set;} 
        public PermissionAttribute(string [] Permissions,bool RequiredAll = false) 
        {
            this.Permissions = Permissions; 
            this.RequiredAll = RequiredAll; 
        }
    }
}
