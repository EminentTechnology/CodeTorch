using System;
using System.Linq;
using CodeTorch.Core;
using Xamarin.Forms;
using CodeTorch.Core.Platform;

namespace CodeTorch.Mobile
{

    public class TableView : Xamarin.Forms.TableView, IView
    {

        TableViewControl _Me = null;
        public TableViewControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (TableViewControl)this.BaseControl;
                }
                return _Me;
            }
        }

        public BaseControl BaseControl { get; set; }
        public Page Page { get; set; }
        public MobileScreen Screen { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public void Init()
        {
            ViewHelper.DefaultViewProperties(this);

            this.Intent = Me.Intent;



            TableRoot root = null;

            string title = String.Empty;

            Device.OnPlatform(
                Android: () =>
                {
                    title = OnPlatformString.GetAndroidValue(Me.Title);
                },
                iOS: () =>
                {
                    title = OnPlatformString.GetiOSValue(Me.Title);
                },
                WinPhone: () =>
                {
                    title = OnPlatformString.GetWinPhoneValue(Me.Title);
                }
             );

                if (String.IsNullOrEmpty(title))
                {
                    root = new TableRoot();
                }
                else
                {
                    root = new TableRoot(title);
                }

                if (Me.Sections.Count > 0)
                {

                    foreach (BaseSection section in Me.Sections)
                    {
                        TableSection sectionControl = new TableSection();

                        

                        

                        ProcessSection(sectionControl, section);

                        root.Add(sectionControl);
                    }
                }

            this.Root = root;
        }

        private void ProcessSection(TableSection sectionControl, BaseSection section)
        {

            if (section is TableSectionControl)
            {
                TableSectionControl tableSection = section as TableSectionControl;

                string sectionTitle = String.Empty;

                Device.OnPlatform(
                    Android: () =>
                    {
                        sectionTitle = OnPlatformString.GetAndroidValue(tableSection.Title);
                    },
                    iOS: () =>
                    {
                        sectionTitle = OnPlatformString.GetiOSValue(tableSection.Title);
                    },
                    WinPhone: () =>
                    {
                        sectionTitle = OnPlatformString.GetWinPhoneValue(tableSection.Title);
                    }
                    );

                    if (!String.IsNullOrEmpty(sectionTitle))
                    {
                        sectionControl.Title = sectionTitle;
                    }
                            

                if (tableSection.Cells.Count > 0)
                {
                    foreach (BaseCell cell in tableSection.Cells)
                    {
                        ProcessCell(sectionControl, tableSection, cell);
                    }
                }

            }
            

        }

        private void ProcessCell(TableSection tableSection, TableSectionControl section,  BaseCell baseCell)
        {
            ICell cell = CellHelper.GetCell(this.Page, Me, section, baseCell);

             

            cell.Init();

            tableSection.Add(cell.GetCell());
            
            

        }
        

        public Xamarin.Forms.View GetView()
        {
            return this as Xamarin.Forms.View;
        }

    }
}
