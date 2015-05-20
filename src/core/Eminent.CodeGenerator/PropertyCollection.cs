using System;

namespace Eminent.CodeGenerator
{
	/// <summary>
	/// Summary description for PropertyCollection.
	/// </summary>
	public class PropertyCollection : System.Collections.CollectionBase
	{
		public PropertyCollection() 
		{

		}

		

		public Property this[int index]
		{
			get
			{
				return (Property) List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		public void Add(Property PropertyItem)
		{
			List.Add(PropertyItem);
		}

		public void Remove(int index)
		{
			// Check to see if there is a widget at the supplied index.
			if (index > Count - 1 || index < 0)
				// If no widget exists, a messagebox is shown and the operation 
				// is cancelled.
			{
				//System.Windows.Forms.MessageBox.Show("Index not valid!");
			}
			else
			{
				List.RemoveAt(index); 
			}
		}

	}
}
