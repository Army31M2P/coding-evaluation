using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MyOrganization
{
    internal abstract class Organization
    {
        private Position root;

        private int _counter;

        public Organization()
        {
            root = CreateOrganization();
            _counter = 0;
        }

        protected abstract Position CreateOrganization();

        /**
         * hire the given person as an employee in the position that has that title
         * 
         * @param person
         * @param title
         * @return the newly filled position or empty if no position has that title
         */
        public Position? Hire(Name person, string title)
        {
            //your code here

            //SRM:In a real system this would be a lookup or some other method to create an employee id. In this case used a global varaible to increment
            _counter++;

            //SRM: to account for white space 
            if (string.IsNullOrWhiteSpace(title))
            {
                title = string.Empty;
            }

            //SRM: create position with employee and counter (identifier)
            var emp = new Employee(_counter, person);
            var position = new Position(title, emp);


            //SRM:Root we need to populate names to titles. this is a fragile design since titles can have typos...
            if (emp != null)
            {
                SetTitleRecursive(title, root, emp);
                return position;
            }


            //SRM: return.
            return position;

        }

        override public string ToString()
        {
            return PrintOrganization(root, "");
        }

        private string PrintOrganization(Position pos, string prefix)
        {
            StringBuilder sb = new StringBuilder(prefix + "+-" + pos.ToString() + "\n");
            foreach (Position p in pos.GetDirectReports())
            {
                sb.Append(PrintOrganization(p, prefix + "  "));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Method to set a users title that is Recursive.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="current"></param>
        /// <param name="emp"></param>
        private void SetTitleRecursive(string title, Position? current, Employee emp)
        {

            if (current?.GetTitle() == title)
            {
                current?.SetEmployee(emp);
                return;
            }
            else
            {
                if (current?.GetDirectReports().IsEmpty == false)
                {
                    foreach (Position reportUser in current.GetDirectReports())
                    {
                        if (reportUser.GetTitle() == title)
                        {
                            reportUser.SetEmployee(emp);
                        }
                        else
                        {
                            if (reportUser.GetDirectReports().IsEmpty == false)
                            {
                                //recursive function here. 
                                SetTitleRecursive(title, reportUser, emp);

                            }
                            else //TODO:SRM this case may not be needed need to do some unit testing in the future to validate..
                            {
                                if (reportUser.GetTitle() == title)
                                {
                                    current.SetEmployee(emp);

                                }
                            }
                        }


                    }
                }
                else
                {
                    if (current?.GetTitle() == title)
                    {
                        current.SetEmployee(emp);

                    }
                }
            }

        }


    }
}
