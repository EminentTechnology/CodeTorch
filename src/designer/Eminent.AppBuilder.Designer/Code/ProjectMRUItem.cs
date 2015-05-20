using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Designer.Code
{
    [Serializable]
    public class ProjectMRUItem: IEquatable<ProjectMRUItem>
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public ProjectMRUItem(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        public override string ToString()
        {
            string projectName = Name;

           

            return projectName;
        }

        bool IEquatable<ProjectMRUItem>.Equals(ProjectMRUItem other)
        {
            return IsEqual(other);
        }

        private bool IsEqual(ProjectMRUItem other)
        {
            if ((this.Name.ToLower() == other.Name.ToLower()) && (this.Path.ToLower() == other.Path.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            bool retVal = false;

            if (obj is ProjectMRUItem)
            {
                retVal = IsEqual((ProjectMRUItem)obj);
            }
            else
            {
                retVal = base.Equals(obj);
            }

            return retVal;

            
        }
    }
}
