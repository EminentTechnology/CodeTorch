using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeTorch.Core;

namespace CodeTorch.Designer.UserControls
{
    public partial class SectionZoneDesigner : UserControl
    {
        public SectionDivider Zone;
        public CodeTorch.Core.Screen Screen;

        public SectionZoneDesigner()
        {
            InitializeComponent();

            
        }
        
        public void InitControl()
        {
            ZoneLabel.DataBindings.Add("Text", Zone, "Name");
            
            
            var sections = Screen.Sections
                                        .Where(s => 
                                            (
                                                (s.ContentPane.ToLower() == Zone.Name.ToLower())
                                            )
                                        );


            SectionsPanel.RowCount = sections.Count();
            foreach (Section section in sections)
            {
                AddSections(section);
            }
        }

        private void AddSections(Section section)
        {
            SectionDesigner s = new SectionDesigner();

            s.Screen = this.Screen;
            s.Zone = this.Zone;
            s.Section = section;
            s.Dock = DockStyle.Fill;
            s.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            s.AutoSize = true;
            //s.Size = new Size(this.Width, 200);
            

            s.InitControl();

            this.SectionsPanel.Controls.Add(s);

            SectionsPanel.RowStyles.Clear();

            for (int i = 0; i < SectionsPanel.Controls.Count; i++ )
            {
                SectionsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, (100/SectionsPanel.Controls.Count)));
            }
        }
    }
}
