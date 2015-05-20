using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;
using System.Collections.Generic;

namespace CodeTorch.Mobile
{

    public class ListView : Xamarin.Forms.ListView, IView
    {

        ListViewControl _Me = null;
        public ListViewControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (ListViewControl)this.BaseControl;
                }
                return _Me;
            }
        }

        public BaseControl BaseControl { get; set; }
        public Page Page { get; set; }
        public MobileScreen Screen { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        protected override Cell CreateDefault(object item)
        {
            //return base.CreateDefault(item);
            if (item is DataRow)
            {
                DataRow row = item as DataRow;

                // Create views with bindings for displaying each property.
                    Label nameLabel = new Label();
                nameLabel.Text = row["Name"].ToString();
                   // nameLabel.SetBinding(Label.TextProperty, "Name");

                    Label birthdayLabel = new Label();
                    birthdayLabel.Text = row["Age"].ToString();
                    //birthdayLabel.SetBinding(Label.TextProperty,
                    //    new Binding("Age", BindingMode.OneWay, 
                    //                null, null, "Born {0:d}"));

                    BoxView boxView = new BoxView();
                    boxView.Color = Color.Blue;
                    //boxView.SetBinding(BoxView.ColorProperty, "FavoriteColor");

                    // Return an assembled ViewCell.
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children = 
                            {
                                boxView,
                                new StackLayout
                                {
                                    VerticalOptions = LayoutOptions.Center,
                                    Spacing = 0,
                                    Children = 
                                    {
                                        nameLabel,
                                        birthdayLabel
                                    }
                                }
                            }
                        }
                    };
            }
            else
            { 
                String str = null;
                if (item != null)
                {
                    str = item.ToString();
                }
                return new TextCell()
                {
                    Text = "ochu" + str 
                };
            }
            
        }

        public void Init()
        {
            // TODO: Implement this method
            //throw new NotImplementedException();

            //default listview for demo purposes
            List<Person> people = new List<Person>()
            {
                new Person("Abigail", new DateTime(1975, 1, 15), Color.Aqua),
                new Person("Bob", new DateTime(1976, 2, 20), Color.Black),
                new Person("Cathy", new DateTime(1977, 3, 10), Color.Blue),
                new Person("David", new DateTime(1978, 4, 25), Color.Fuschia),
                new Person("Eugenie", new DateTime(1979, 5, 5), Color.Gray),
                new Person("Freddie", new DateTime(1980, 6, 30), Color.Green),
                new Person("Greta", new DateTime(1981, 7, 15), Color.Lime),
                new Person("Harold", new DateTime(1982, 8, 10), Color.Maroon),
                new Person("Irene", new DateTime(1983, 9, 25), Color.Navy),
                new Person("Jonathan", new DateTime(1984, 10, 10), Color.Olive),
                new Person("Kathy", new DateTime(1985, 11, 20), Color.Purple),
                new Person("Larry", new DateTime(1986, 12, 5), Color.Red),
                new Person("Monica", new DateTime(1975, 1, 5), Color.Silver),
                new Person("Nick", new DateTime(1976, 2, 10), Color.Teal),
                new Person("Olive", new DateTime(1977, 3, 20), Color.White),
                new Person("Pendleton", new DateTime(1978, 4, 10), Color.Yellow),
                new Person("Queenie", new DateTime(1979, 5, 15), Color.Aqua),
                new Person("Rob", new DateTime(1980, 6, 30), Color.Blue),
                new Person("Sally", new DateTime(1981, 7, 5), Color.Fuschia),
                new Person("Timothy", new DateTime(1982, 8, 30), Color.Green),
                new Person("Uma", new DateTime(1983, 9, 10), Color.Lime),
                new Person("Victor", new DateTime(1984, 10, 20), Color.Maroon),
                new Person("Wendy", new DateTime(1985, 11, 5), Color.Navy),
                new Person("Xavier", new DateTime(1986, 12, 30), Color.Olive),
                new Person("Yvonne", new DateTime(1987, 1, 10), Color.Purple),
                new Person("Zachary", new DateTime(1988, 2, 5), Color.Red)
            };

            DataTable dt = new DataTable();

            DataRow row = null;
            
            row = new DataRow();
            row["Name"] = "Philip";
            row["Age"] = 20;
            dt.Rows.Add(row);

            row = new DataRow();
            row["Name"] = "Omo";
            row["Age"] = 22;
            dt.Rows.Add(row);

            var data = dt.Rows.Select(r =>
        new
            {
                Name = (int)r["Name"],
                Age = (string)r["Age"]
            });

            Type t = Type.GetType("BeStill.Models.TestItem");
            if (t != null)
            { 
                //object o = Activator.CreateInstance(t);
                //o.
            }
            

            this.ItemsSource = dt.Rows; ;
            //this.ItemsSource = new [] { "a", "b", "c" };

            //this.ItemTemplate = new DataTemplate(() =>
            //    {
            //        // Create views with bindings for displaying each property.
            //        Label nameLabel = new Label();
            //        nameLabel.SetBinding(Label.TextProperty, "Name");

            //        Label birthdayLabel = new Label();
            //        birthdayLabel.SetBinding(Label.TextProperty,
            //            new Binding("Age", BindingMode.OneWay, 
            //                        null, null, "Born {0:d}"));

            //        //BoxView boxView = new BoxView();
            //        //boxView.SetBinding(BoxView.ColorProperty, "FavoriteColor");

            //        // Return an assembled ViewCell.
            //        return new ViewCell
            //        {
            //            View = new StackLayout
            //            {
            //                Padding = new Thickness(0, 5),
            //                Orientation = StackOrientation.Horizontal,
            //                Children = 
            //                {
            //                    //boxView,
            //                    new StackLayout
            //                    {
            //                        VerticalOptions = LayoutOptions.Center,
            //                        Spacing = 0,
            //                        Children = 
            //                        {
            //                            nameLabel,
            //                            birthdayLabel
            //                        }
            //                    }
            //                }
            //            }
            //        };
            //    });
            

        }

        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

        class Person
        {
            public Person(string name, DateTime birthday, Color favoriteColor)
            {
                this.Name = name;
                this.Birthday = birthday;
                this.FavoriteColor = favoriteColor;
            }

            public string Name { private set; get; }

            public DateTime Birthday { private set; get; }

            public Color FavoriteColor { private set; get; }
        };

        

    }
}
