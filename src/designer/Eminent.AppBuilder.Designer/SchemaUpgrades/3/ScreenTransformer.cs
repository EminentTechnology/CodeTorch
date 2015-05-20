using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Designer.SchemaUpgrades._3
{
    class ScreenTransformer: ICodeTransformer
    {
        

        public bool Execute()
        {
            ProcessScreen(Document);
            return true;
        }

        public string EntityType {get;set;}
        public XDocument Document { get; set; }

        public List<string> GetSupportedEntityTypes()
        {
            List<String> types = new List<string>();

            types.Add("Screen");

            return types;
        }
        
        public void GenerateRequiredObjects(string configPath)
        {

            


            Generate_Comment_GetCommentsByEntityIDDataCommand(configPath);
            Generate_Comment_SaveCommentDataCommand(configPath);
            Generate_Document_GetDocumentsByEntityIDDataCommand(configPath);
            Generate_Document_UpdateEntityDataCommand(configPath);
            Generate_Document_InActivate(configPath);
            Generate_Documents_DownloadScreen(configPath);
            Generate_User_GetByUserNameDataCommand(configPath);
            Generate_SectionZoneLayouts(configPath);
            Generate_Document_GetByIDDataCommand(configPath);

        }

        private static void Generate_User_GetByUserNameDataCommand(string configPath)
        {
            string dataCommandName = "User_GetByUserName";
            string filePath = String.Format("{0}/DataCommands/{1}.xml", configPath, dataCommandName);

            if (!File.Exists(filePath))
            {
                XDocument doc = new XDocument(
                        new XElement("DataCommand",
                                new XElement("Name", dataCommandName),
                                new XElement("Type", "Text"),
                                new XElement("Text", "SELECT UserID, UserName, Password, FirstName, LastName, Status FROM Users WHERE UserName=@UserName and Status=1"),
                                new XElement("ReturnType", "DataTable"),
                                new XElement("Parameters",
                                        new XElement("Parameter",
                                                new XElement("Name", "@UserName"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            )

                                    ),
                                new XElement("Columns",
                                        new XElement("Column",
                                            new XElement("Name", "UserID"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "UserName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "Password"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "FirstName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "LastName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "Status"),
                                            new XElement("Type", "Int32")
                                        )
                               )
                         )
                    );

                doc.Save(filePath);

            }
        }

        private static void Generate_Documents_DownloadScreen(string configPath)
        {
            string folder = "Document";
            string screen = "Download.aspx";
            string dirpath = String.Format(@"{0}\Screens\{1}\", configPath, folder);
            string filePath = String.Format(@"{0}\Screens\{1}\{2}.xml", configPath, folder, screen);

         
            if (!File.Exists(filePath))
            {
                XDocument doc = new XDocument(
                        new XElement("Screen",
                            new XElement("Name", screen),
                            new XElement("Type", "Screen"),
                            new XElement("Folder", folder),
                            new XElement("RequireAuthentication", "true"),
                            new XElement("ValidateRequest", "true"),
                            new XElement("ScreenPermission",
                                    new XElement("CheckPermission", "false")
                                ),
                            new XElement("DataCommands",
                                        new XElement("DataCommand",
                                          new XElement("Name", "Document_GetByID"),
                                          new XElement("Parameters",
                                                new XElement("Parameter",
                                                        new XElement("Name", "@DocumentID"),
                                                        new XElement("InputType", "QueryString"),
                                                        new XElement("InputKey", "DocumentID")
                                                    )
                                              )
                                        )
                                    ),
                            new XElement("OnPageLoad",
                                    new XElement("Commands",
                                        new XElement("DownloadDocumentCommand",
                                                new XElement("Name", "Download"),
                                                new XElement("RetrieveCommand", "Document_GetByID")
                                            )
                                        )
                                    )
                        )
                    );

                if (!Directory.Exists(dirpath))
                {
                    Directory.CreateDirectory(dirpath);
                }

                doc.Save(filePath);

            }
        }

        private static void Generate_Comment_GetCommentsByEntityIDDataCommand(string configPath)
        {
            string dataCommandName = "Comment_GetCommentsByEntityID";
            string filePath = String.Format("{0}/DataCommands/{1}.xml", configPath, dataCommandName);

            if (!File.Exists(filePath))
            {
                XDocument doc = new XDocument(
                        new XElement("DataCommand",
                                new XElement("Name", dataCommandName),
                                new XElement("Type", "Text"),
                                new XElement("Text", "SELECT c.CreatedBy as FullName, c.CreatedBy as FirstName, c.CreatedBy as LastName, c.CommentID,c.EntityID,c.EntityType,c.Comments,c.Status,c.CreatedOn,c.CreatedBy,c.ModifiedOn,c.ModifiedBy FROM Comment c   WHERE EntityID = @EntityID AND EntityType=@EntityType Order By CreatedOn Desc"),
                                new XElement("ReturnType", "DataTable"),
                                new XElement("Parameters",
                                        new XElement("Parameter",
                                                new XElement("Name", "@EntityID"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            ),
                                        new XElement("Parameter",
                                                new XElement("Name", "@EntityType"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            )
                                    ),
                                new XElement("Columns",
                                        new XElement("Column",
                                            new XElement("Name", "FullName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "FirstName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "LastName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "CommentID"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "EntityID"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "EntityType"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "Comments"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "Status"),
                                            new XElement("Type", "Int32")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "CreatedOn"),
                                            new XElement("Type", "DateTime")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "CreatedBy"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "ModifiedOn"),
                                            new XElement("Type", "DateTime")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "ModifiedBy"),
                                            new XElement("Type", "String")
                                        )
                               )
                         )
                    );

                doc.Save(filePath);

            }
        }

        private static void Generate_Document_GetDocumentsByEntityIDDataCommand(string configPath)
        {
            string dataCommandName = "Document_GetDocumentsByEntityID";
            string filePath = String.Format("{0}/DataCommands/{1}.xml", configPath, dataCommandName);

            if (!File.Exists(filePath))
            {
                XDocument doc = new XDocument(
                        new XElement("DataCommand",
                                new XElement("Name", dataCommandName),
                                new XElement("Type", "Text"),
                                new XElement("Text", "SELECT u.FirstName + ' ' + u.LastName as FullName,u.FirstName, u.LastName, d.DocumentID,d.DocumentTypeCode,dt.LookupDescription Description, d.DocumentName,d.EntityID,d.EntityType,d.ContentType,d.Size,d.CreatedOn,d.CreatedBy,d.ModifiedOn,d.ModifiedBy FROM Document d  LEFT JOIN Users u ON (d.CreatedBy = u.UserName) LEFT Join Lookup dt ON (dt.LookupType='DOCUMENT_TYPE') AND (d.DocumentTypeCode = dt.LookupValue) WHERE EntityType=@EntityType AND EntityID = @EntityID AND d.Status=1 Order By CreatedOn Desc"),
                                new XElement("ReturnType", "DataTable"),
                                new XElement("Parameters",
                                        new XElement("Parameter",
                                                new XElement("Name", "@EntityType"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            ),
                                            new XElement("Parameter",
                                                new XElement("Name", "@EntityID"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            )

                                    ),
                                new XElement("Columns",
                                        new XElement("Column",
                                            new XElement("Name", "FullName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "FirstName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "LastName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "DocumentID"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "DocumentTypeCode"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "Description"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "DocumentName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "EntityID"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "EntityType"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "ContentType"),
                                            new XElement("Type", "String")
                                        ),
                                         new XElement("Column",
                                            new XElement("Name", "Size"),
                                            new XElement("Type", "Int32")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "CreatedOn"),
                                            new XElement("Type", "DateTime")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "CreatedBy"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "ModifiedOn"),
                                            new XElement("Type", "DateTime")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "ModifiedBy"),
                                            new XElement("Type", "String")
                                        )
                               )
                         )
                    );

                doc.Save(filePath);

            }
        }

        private static void Generate_Document_GetByIDDataCommand(string configPath)
        {
            string dataCommandName = "Document_GetByID";
            string filePath = String.Format("{0}/DataCommands/{1}.xml", configPath, dataCommandName);

            if (!File.Exists(filePath))
            {
                XDocument doc = new XDocument(
                        new XElement("DataCommand",
                                new XElement("Name", dataCommandName),
                                new XElement("Type", "Text"),
                                new XElement("Text", "SELECT d.DocumentID, d.StorageType, d.EntityID, d.EntityType, d.DocumentName, d.DocumentTypeCode, d.DocumentUrl, d.[File], d.ContentType, d.Size, d.IsDefault, d.Status, d.CreatedOn, d.CreatedBy, d.ModifiedOn, d.ModifiedBy FROM Document d WHERE d.DocumentID = @DocumentID"),
                                new XElement("ReturnType", "DataTable"),
                                new XElement("Parameters",
                                            new XElement("Parameter",
                                                new XElement("Name", "@DocumentID"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            )

                                    ),
                                new XElement("Columns",
                                        new XElement("Column",
                                            new XElement("Name", "DocumentID"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "StorageType"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "EntityID"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "EntityType"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "DocumentName"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "DocumentTypeCode"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "DocumentUrl"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "File"),
                                            new XElement("Type", "Byte[]")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "ContentType"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "Size"),
                                            new XElement("Type", "Int32")
                                        ),
                                         new XElement("Column",
                                            new XElement("Name", "IsDefault"),
                                            new XElement("Type", "Boolean")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "Status"),
                                            new XElement("Type", "Int32")
                                        ),
                                         new XElement("Column",
                                            new XElement("Name", "CreatedOn"),
                                            new XElement("Type", "DateTime")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "CreatedBy"),
                                            new XElement("Type", "String")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "ModifiedOn"),
                                            new XElement("Type", "DateTime")
                                        ),
                                        new XElement("Column",
                                            new XElement("Name", "ModifiedBy"),
                                            new XElement("Type", "String")
                                        )
                               )
                         )
                    );

                doc.Save(filePath);

            }
        }

        private static void Generate_Comment_SaveCommentDataCommand(string configPath)
        {
            string dataCommandName = "Comment_SaveComment";
            string filePath = String.Format("{0}/DataCommands/{1}.xml", configPath, dataCommandName);

            if (!File.Exists(filePath))
            {
                XDocument doc = new XDocument(
                        new XElement("DataCommand",
                                new XElement("Name", dataCommandName),
                                new XElement("Type", "Text"),
                                new XElement("Text", "INSERT Comment (CommentID, EntityID, EntityType, Comments, Status, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy) VALUES (NewID(), @EntityID, @EntityType, @Comments, 1, getdate(), @CreatedBy, getdate(), @CreatedBy)"),
                                new XElement("ReturnType", "Integer"),
                                new XElement("Parameters",
                                        new XElement("Parameter",
                                                new XElement("Name", "@EntityID"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            ),
                                        new XElement("Parameter",
                                                new XElement("Name", "@EntityType"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            ),
                                        new XElement("Parameter",
                                                new XElement("Name", "@Comments"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "-1"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            ),
                                        new XElement("Parameter",
                                                new XElement("Name", "@CreatedBy"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "255"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            )
                                    )
                         )
                    );

                doc.Save(filePath);

            }
        }

        private static void Generate_Document_UpdateEntityDataCommand(string configPath)
        {
            string dataCommandName = "Document_UpdateEntity";
            string filePath = String.Format("{0}/DataCommands/{1}.xml", configPath, dataCommandName);

            if (!File.Exists(filePath))
            {
                XDocument doc = new XDocument(
                        new XElement("DataCommand",
                                new XElement("Name", dataCommandName),
                                new XElement("Type", "Text"),
                                new XElement("Text", "UPDATE Document SET EntityID = @EntityID, EntityType = @EntityType, IsDefault = 0, ModifiedOn = GetDate(), ModifiedBy = @ModifiedBy WHERE DocumentID = @DocumentID"),
                                new XElement("ReturnType", "Integer"),
                                new XElement("Parameters",
                                        new XElement("Parameter",
                                                new XElement("Name", "@EntityID"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            ),
                                        new XElement("Parameter",
                                                new XElement("Name", "@EntityType"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            ),
                                        new XElement("Parameter",
                                                new XElement("Name", "@DocumentID"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "50"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            ),
                                        new XElement("Parameter",
                                                new XElement("Name", "@ModifiedBy"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "255"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            )
                                    )
                         )
                    );

                doc.Save(filePath);

            }
        }

        private static void Generate_Document_InActivate(string configPath)
        {
            string dataCommandName = "Document_Inactivate";
            string filePath = String.Format("{0}/DataCommands/{1}.xml", configPath, dataCommandName);

            if (!File.Exists(filePath))
            {
                XDocument doc = new XDocument(
                        new XElement("DataCommand",
                                new XElement("Name", dataCommandName),
                                new XElement("Type", "Text"),
                                new XElement("Text", "UPDATE Document SET IsDefault = 0, Status = 0, ModifiedOn = GetDate(), 	ModifiedBy = @ModifiedBy WHERE DocumentID = @DocumentID"),
                                new XElement("ReturnType", "Integer"),
                                new XElement("Parameters",
                                        new XElement("Parameter",
                                                new XElement("Name", "@DocumentID"),
                                                new XElement("Type", "Guid"),
                                                new XElement("Size", "200"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "uniqueidentifier"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            ),
                                        new XElement("Parameter",
                                                new XElement("Name", "@ModifiedBy"),
                                                new XElement("Type", "String"),
                                                new XElement("Size", "255"),
                                                new XElement("Direction", "In"),
                                                new XElement("TypeName", "nvarchar"),
                                                new XElement("IsUserDefinedType", "false"),
                                                new XElement("IsTableType", "false")
                                            )
                                    )
                         )
                    );

                doc.Save(filePath);

            }
        }

     
        private static void Generate_SectionZoneLayouts(string configPath)
        {
            string layout = null; ;
            string filePath = null;
            XDocument doc = null;

            layout = "SingleColumn";
            filePath = String.Format("{0}/SectionZoneLayouts/{1}.xml", configPath, layout);
            doc = new XDocument(
                new XElement("SectionZoneLayout",
                        new XElement("Name", layout),
                        new XElement("Dividers",
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Top"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Left"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Bottom"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    )
                            )
                    )
            );
            doc.Save(filePath);
            
            layout = "Default";
            filePath = String.Format("{0}/SectionZoneLayouts/{1}.xml", configPath, layout);
            doc = new XDocument(
                new XElement("SectionZoneLayout",
                        new XElement("Name", layout),
                        new XElement("Dividers",
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Top"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Left"),
                                                new XElement("CssClass", "col-md-6")
                                            ),
                                            new XElement("Divider",
                                                new XElement("Name", "Right"),
                                                new XElement("CssClass", "col-md-6")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Bottom"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    )
                            )
                    )
            );
            doc.Save(filePath);


            layout = "DefaultPlusFooter";
            filePath = String.Format("{0}/SectionZoneLayouts/{1}.xml", configPath, layout);
            doc = new XDocument(
                new XElement("SectionZoneLayout",
                        new XElement("Name", layout),
                        new XElement("Dividers",
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Top"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Left"),
                                                new XElement("CssClass", "col-md-6")
                                            ),
                                            new XElement("Divider",
                                                new XElement("Name", "Right"),
                                                new XElement("CssClass", "col-md-6")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Footer"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Bottom"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    )
                            )
                    )
            );
            doc.Save(filePath);

            layout = "DefaultPlusHeaderFooter";
            filePath = String.Format("{0}/SectionZoneLayouts/{1}.xml", configPath, layout);
            doc = new XDocument(
                new XElement("SectionZoneLayout",
                        new XElement("Name", layout),
                        new XElement("Dividers",
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Top"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Header"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Left"),
                                                new XElement("CssClass", "col-md-6")
                                            ),
                                            new XElement("Divider",
                                                new XElement("Name", "Right"),
                                                new XElement("CssClass", "col-md-6")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Footer"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Bottom"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    )
                            )
                    )
            );
            doc.Save(filePath);
            //
            layout = "ThreeColumn";
            filePath = String.Format("{0}/SectionZoneLayouts/{1}.xml", configPath, layout);
            doc = new XDocument(
                new XElement("SectionZoneLayout",
                        new XElement("Name", layout),
                        new XElement("Dividers",
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Top"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Left"),
                                                new XElement("CssClass", "col-md-4")
                                            ),
                                            new XElement("Divider",
                                                new XElement("Name", "Center"),
                                                new XElement("CssClass", "col-md-4")
                                            ),
                                            new XElement("Divider",
                                                new XElement("Name", "Right"),
                                                new XElement("CssClass", "col-md-4")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Bottom"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    )
                            )
                    )
            );
            doc.Save(filePath);


            layout = "ThreeColumnPlusFooter";
            filePath = String.Format("{0}/SectionZoneLayouts/{1}.xml", configPath, layout);
            doc = new XDocument(
                new XElement("SectionZoneLayout",
                        new XElement("Name", layout),
                        new XElement("Dividers",
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Top"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Left"),
                                                new XElement("CssClass", "col-md-4")
                                            ),
                                            new XElement("Divider",
                                                new XElement("Name", "Center"),
                                                new XElement("CssClass", "col-md-4")
                                            ),
                                            new XElement("Divider",
                                                new XElement("Name", "Right"),
                                                new XElement("CssClass", "col-md-4")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Footer"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Bottom"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    )
                            )
                    )
            );
            doc.Save(filePath);

            layout = "ThreeColumnPlusHeaderFooter";
            filePath = String.Format("{0}/SectionZoneLayouts/{1}.xml", configPath, layout);
            doc = new XDocument(
                new XElement("SectionZoneLayout",
                        new XElement("Name", layout),
                        new XElement("Dividers",
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Top"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Header"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Left"),
                                                new XElement("CssClass", "col-md-4")
                                            ),
                                            new XElement("Divider",
                                                new XElement("Name", "Center"),
                                                new XElement("CssClass", "col-md-4")
                                            ),
                                            new XElement("Divider",
                                                new XElement("Name", "Right"),
                                                new XElement("CssClass", "col-md-4")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Footer"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    ),
                                new XElement("Divider",
                                        new XElement("CssClass", "row"),
                                        new XElement("Dividers",
                                            new XElement("Divider",
                                                new XElement("Name", "Bottom"),
                                                new XElement("CssClass", "col-md-12")
                                            )
                                        )
                                    )
                            )
                    )
            );
            doc.Save(filePath);
            
        }

        

        public  void ProcessScreen(XDocument Document)
        {


            CreateSections(Document);



            switch (Document.Root.Name.LocalName.ToLower())
            {


                case "listscreen":
                    CreateAlertsSection(Document, false);
                    ConvertActionLinksToLinksSection(Document);
                    ConvertGridToGridSection(Document);
                    ProcessListScreen(Document);
                    AddSimpleRenderPageSectionsCommand(Document);
                    break;

                case "searchscreen":
                    CreateAlertsSection(Document, false);
                    ConvertActionLinksToLinksSection(Document);
                    ConvertCriteriaToCriteriaSection(Document, false);
                    ConvertGridToGridSection(Document);
                    ProcessSearchScreen(Document);
                    AddSimpleRenderPageSectionsCommand(Document);
                    break;

                case "editscreen":
                    CreateAlertsSection(Document, true);
                    ProcessEditScreen(Document);

                    break;

                case "listeditscreen":
                    ConvertGridToEditableGridSection(Document);
                    CreateAlertsSection(Document, true);
                    AddSimpleRenderPageSectionsCommand(Document);
                    break;
                case "criterialisteditscreen":
                    ConvertGridToEditableGridSection(Document);
                    ConvertCriteriaToCriteriaSection(Document, true);
                    CreateAlertsSection(Document, true);
                    ProcessCriteriaListEditScreen(Document);
                    AddSimpleRenderPageSectionsCommand(Document);
                    break;
                case "pickerscreen":
                    ConvertGridToEditableGridSection(Document);
                    ConvertCriteriaToCriteriaSection(Document, true);
                    CreateAlertsSection(Document, true);
                    ProcessPickerScreen(Document);
                    AddSimpleRenderPageSectionsCommand(Document);
                    break;
                case "commentsscreen":
                    CreateAlertsSection(Document, true);
                    ProcessCommentsScreen(Document);

                    break;
                case "contentscreen":
                    CreateAlertsSection(Document, true);
                    ProcessContentScreen(Document);
                    AddSimpleRenderPageSectionsCommand(Document);
                    break;
                case "documentsscreen":
                    CreateAlertsSection(Document, true);
                    ProcessDocumentsScreen(Document);

                    break;
                case "overviewscreen":
                    CreateAlertsSection(Document, false);
                    ProcessOverviewScreen(Document);
                    AddSimpleRenderPageSectionsCommand(Document);
                    break;
                case "logoutscreen":

                    ProcessLogoutScreen(Document);

                    break;
                case "loginscreen":
                    CreateAlertsSection(Document, true);
                    ProcessLoginScreen(Document);
                    AddSimpleRenderPageSectionsCommand(Document);
                    break;
                case "redirectscreen":

                    ProcessRedirectScreen(Document);

                    break;
                case "reportexportscreen":

                    ProcessReportExportScreen(Document);

                    break;
                case "reportviewerscreen":
                    CreateAlertsSection(Document, false);
                    ConvertCriteriaToCriteriaSection(Document, false);
                    ProcessReportViewerScreen(Document);
                    AddSimpleRenderPageSectionsCommand(Document);

                    break;
            }

            ConvertSectionAndControlActions(Document);
            ConvertAfterPopulateScreenToPageLoad(Document);

            Document.Root.Name = "Screen";
            Document.Root.Element("Type").Value = "Screen";

        }

        private static void ProcessLoginScreen(XDocument Document)
        {
            string ProfileCommand = "User_GetByUserName";
            XElement ProfileCommandElement = Document.Root.Element("ProfileCommand");
            if (ProfileCommandElement != null)
            {
                ProfileCommand = ProfileCommandElement.Value;
                ProfileCommandElement.Remove();
            }

            string PasswordField = "Password";
            XElement PasswordFieldElement = Document.Root.Element("PasswordField");
            if (PasswordFieldElement != null)
            {
                PasswordField = PasswordFieldElement.Value;
                PasswordFieldElement.Remove();
            }

            string UserNameParameter = "@UserName";
            XElement UserNameParameterElement = Document.Root.Element("UserNameParameter");
            if (UserNameParameterElement != null)
            {
                UserNameParameter = UserNameParameterElement.Value;
                UserNameParameterElement.Remove();
            }

            string PasswordAlgorithm = "SaltHashProvider";
            XElement PasswordAlgorithmElement = Document.Root.Element("PasswordAlgorithm");
            if (PasswordAlgorithmElement != null)
            {
                PasswordAlgorithm = PasswordAlgorithmElement.Value;
                PasswordAlgorithmElement.Remove();
            }

            string PasswordMode = "Hash";
            XElement PasswordModeElement = Document.Root.Element("PasswordMode");
            if (PasswordModeElement != null)
            {
                PasswordMode = PasswordModeElement.Value;
                PasswordModeElement.Remove();
            }

            string LogoutTimeout = "30";
            XElement LogoutTimeoutElement = Document.Root.Element("LogoutTimeout");
            if (LogoutTimeoutElement != null)
            {
                LogoutTimeout = LogoutTimeoutElement.Value;
                LogoutTimeoutElement.Remove();
            }

            string DisplaySigninLink = "";
            XElement DisplaySigninLinkElement = Document.Root.Element("DisplaySigninLink");
            if (DisplaySigninLinkElement != null)
            {
                DisplaySigninLink = DisplaySigninLinkElement.Value;
                DisplaySigninLinkElement.Remove();
            }

            string SigninText = "";
            XElement SigninTextElement = Document.Root.Element("SigninText");
            if (SigninTextElement != null)
            {
                SigninText = SigninTextElement.Value;
                SigninTextElement.Remove();
            }

            string SigninUrl = "";
            XElement SigninUrlElement = Document.Root.Element("SigninUrl");
            if (SigninUrlElement != null)
            {
                SigninUrl = SigninUrlElement.Value;
                SigninUrlElement.Remove();
            }

            string DisplayForgetPasswordLink = "";
            XElement DisplayForgetPasswordLinkElement = Document.Root.Element("DisplayForgetPasswordLink");
            if (DisplayForgetPasswordLinkElement != null)
            {
                DisplayForgetPasswordLink = DisplayForgetPasswordLinkElement.Value;
                DisplayForgetPasswordLinkElement.Remove();
            }

            string ForgetPasswordText = "";
            XElement ForgetPasswordTextElement = Document.Root.Element("ForgetPasswordText");
            if (ForgetPasswordTextElement != null)
            {
                ForgetPasswordText = ForgetPasswordTextElement.Value;
                ForgetPasswordTextElement.Remove();
            }

            string ForgetPasswordUrl = "";
            XElement ForgetPasswordUrlElement = Document.Root.Element("ForgetPasswordUrl");
            if (ForgetPasswordUrlElement != null)
            {
                ForgetPasswordUrl = ForgetPasswordUrlElement.Value;
                ForgetPasswordUrlElement.Remove();
            }

            string UserNameLabel = "";
            XElement UserNameLabelElement = Document.Root.Element("UserNameLabel");
            if (UserNameLabelElement != null)
            {
                UserNameLabel = UserNameLabelElement.Value;
                UserNameLabelElement.Remove();
            }
            if (String.IsNullOrEmpty(UserNameLabel))
                UserNameLabel = "Username";

            string PasswordLabel = "";
            XElement PasswordLabelElement = Document.Root.Element("PasswordLabel");
            if (PasswordLabelElement != null)
            {
                PasswordLabel = PasswordLabelElement.Value;
                PasswordLabelElement.Remove();
            }
            if (String.IsNullOrEmpty(PasswordLabel))
                PasswordLabel = "Password";

            string UserNamePlaceholder = "";
            XElement UserNamePlaceholderElement = Document.Root.Element("UserNamePlaceholder");
            if (UserNamePlaceholderElement != null)
            {
                UserNamePlaceholder = UserNamePlaceholderElement.Value;
                UserNamePlaceholderElement.Remove();
            }

            string PasswordPlaceholder = "";
            XElement PasswordPlaceholderElement = Document.Root.Element("PasswordPlaceholder");
            if (PasswordPlaceholderElement != null)
            {
                PasswordPlaceholder = PasswordPlaceholderElement.Value;
                PasswordPlaceholderElement.Remove();
            }

            //add edit section
            XElement loginSection = new XElement("EditSection",
                    new XElement("Name", "Login"),
                    new XElement("ID", "LoginSection"),
                    new XElement("Type", "Edit"),
                    new XElement("Mode", "All"),
                    new XElement("Visible", true),
                    new XElement("ContentPane", "Left"),
                    new XElement("Permission",
                        new XElement("CheckPermission", false)
                    ),
                    new XElement("Controls")
                );

            XElement emailElement = new XElement("EmailAddressControl",
                new XElement("Name", UserNameParameter.Replace("@", "")),
                new XElement("Type", "EmailAddress"),
                new XElement("Label", UserNameLabel),
                new XElement("IsRequired", "true"),
                new XElement("Visible", "true"),
                new XElement("LabelRenderingMode", "Top")
                );

            XElement usernameElement = new XElement("TextBoxControl",
                new XElement("Name", UserNameParameter.Replace("@", "")),
                new XElement("Type", "TextBox"),
                new XElement("Label", UserNameLabel),
                new XElement("IsRequired", "true"),
                new XElement("Visible", "true"),
                new XElement("LabelRenderingMode", "Top")
                );

            XElement passwordElement = new XElement("PasswordControl",
                new XElement("Name", "Password"),
                new XElement("Type", "Password"),
                new XElement("Label", PasswordLabel),
                new XElement("IsRequired", "true"),
                new XElement("Visible", "true"),
                new XElement("LabelRenderingMode", "Top"),
                new XElement("PasswordAlgorithm", PasswordAlgorithm),
                new XElement("PasswordMode", "PlainText")
                );

            if (UserNameLabel.ToLower().Contains("email"))
            {
                loginSection.Element("Controls").Add(emailElement);
            }
            else
            {
                loginSection.Element("Controls").Add(usernameElement);
            }
            loginSection.Element("Controls").Add(passwordElement);
            Document.Root.Element("Sections").Add(loginSection);

            XElement loginButton = new XElement("Button",
                    new XElement("Name", "Login"),
                    new XElement("Text", "Login"),
                    new XElement("CssClass", "btn btn-lg btn-success btn-block"),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "true"),
                    new XElement("OnClick",
                        new XElement("Commands",
                            new XElement("ValidateUserCommand",
                                new XElement("Name", "SaveCommand"),
                                new XElement("ProfileCommand", ProfileCommand),
                                new XElement("UserNameParameter", UserNameParameter),
                                new XElement("PasswordEntityID", "Password"),
                                new XElement("PasswordEntityInputType", "Control"),
                                new XElement("PasswordField", PasswordField),
                                new XElement("PasswordAlgorithm", PasswordAlgorithm),
                                new XElement("PasswordMode", PasswordMode),
                                new XElement("LogoutTimeout", LogoutTimeout)
                                )
                            )
                        )
                    );



            XElement buttonSection = new XElement("ButtonListSection",
                new XElement("Name", "Buttons"),
                new XElement("Type", "ButtonList"),
                new XElement("ContainerMode", "Plain"),
                new XElement("CssClass", "text-center"),
                new XElement("Mode", "All"),
                new XElement("Visible", true),
                new XElement("ContentPane", "Bottom"),
                new XElement("Permission",
                    new XElement("CheckPermission", false)
                ),
                new XElement("Buttons",
                    new XElement("CheckPermission", false),
                    loginButton
                )
            );

            Document.Root.Element("Sections").Add(buttonSection);

            if (!String.IsNullOrEmpty(ForgetPasswordText) || !String.IsNullOrEmpty(SigninText))
            {
                StringBuilder s = new StringBuilder();
                s.AppendLine("<div class='clearfix' style='padding-bottom:20px;'>");

                if (!String.IsNullOrEmpty(DisplayForgetPasswordLink) && (DisplayForgetPasswordLink.ToLower() == "true"))
                {
                    if (ForgetPasswordUrl.StartsWith("~"))
                        ForgetPasswordUrl = ForgetPasswordUrl.Replace("~", "../..");

                    s.AppendFormat("<div class='pull-left'><a href='{0}'>{1}</a></div>", ForgetPasswordUrl, ForgetPasswordText);
                }

                if (!String.IsNullOrEmpty(DisplaySigninLink) && (DisplaySigninLink.ToLower() == "true"))
                {
                    if (SigninUrl.StartsWith("~"))
                        SigninUrl = ForgetPasswordUrl.Replace("~", "../..");

                    s.AppendFormat("<div class='pull-right'><a href='{0}'>{1}</a></div>", SigninUrl, SigninText);
                }

                s.AppendLine("</div>");

                //s.Replace("<","&lt;");
                //s.Replace(">","&gt;");

                XElement contentSection = new XElement("ContentSection",
                    new XElement("Name", "extra"),
                    new XElement("Type", "Content"),
                    new XElement("ContainerMode", "None"),
                    new XElement("Mode", "All"),
                    new XElement("Visible", true),
                    new XElement("ContentPane", "Left"),
                    new XElement("Permission",
                        new XElement("CheckPermission", false)
                    ),
                    new XElement("Content", s.ToString())

                );

                Document.Root.Element("Sections").Add(contentSection);

            }

            if (Document.Root.Element("DataCommands") == null)
            {
                Document.Root.Add(new XElement("DataCommands"));
            }

            XElement user_GetByUserName_dataCommand = new XElement("DataCommand",
                  new XElement("Name", "User_GetByUserName"),
                  new XElement("Parameters",
                        new XElement("Parameter",
                                new XElement("Name", "@UserName"),
                                new XElement("InputType", "Control"),
                                new XElement("InputKey", UserNameParameter.Replace("@", ""))
                            )
                      )
                );



            Document.Root.Element("DataCommands").Add(user_GetByUserName_dataCommand);

        }

        private static void ProcessReportViewerScreen(XDocument Document)
        {
            //add OnPageLoad action for edit screen
            bool UseDefaultCommand = false;
            string DefaultCommand = String.Empty;
            string RetrieveCommand = String.Empty;

            string SectionHeader = "";
            XElement SectionHeaderElement = Document.Root.Element("SectionHeader");
            if (SectionHeaderElement != null)
            {
                SectionHeader = SectionHeaderElement.Value;
                SectionHeaderElement.Remove();
            }

            XElement rdlcSection = new XElement("RDLCViewerSection",
                    new XElement("Name", SectionHeader),
                    new XElement("Type", "RDLCViewer"),
                    new XElement("Mode", "All"),
                    new XElement("Visible", true),
                    new XElement("ContentPane", "Left"),
                    new XElement("Permission",
                        new XElement("CheckPermission", false)
                    )
                );

            XElement UseDefaultCommandElement = Document.Root.Element("UseDefaultCommand");
            if (UseDefaultCommandElement != null)
            {
                UseDefaultCommand = bool.Parse(UseDefaultCommandElement.Value);
                UseDefaultCommandElement.Remove();
            }

            XElement DefaultCommandElement = Document.Root.Element("DefaultCommand");
            if (DefaultCommandElement != null)
            {
                DefaultCommand = DefaultCommandElement.Value;
                DefaultCommandElement.Remove();
            }

            XElement RetrieveCommandElement = Document.Root.Element("RetrieveCommand");
            if (RetrieveCommandElement != null)
            {
                RetrieveCommand = RetrieveCommandElement.Value;
                RetrieveCommandElement.Remove();
            }

            XElement ReportName = Document.Root.Element("ReportName");
            if (ReportName != null)
            {
                ReportName.Remove();
                rdlcSection.Add(ReportName);
            }

            XElement EnableHyperlinks = Document.Root.Element("EnableHyperlinks");
            if (EnableHyperlinks != null)
            {
                EnableHyperlinks.Remove();
                rdlcSection.Add(EnableHyperlinks);
            }

            XElement EnableLocalization = Document.Root.Element("EnableLocalization");
            if (EnableLocalization != null)
            {
                EnableLocalization.Remove();
                rdlcSection.Add(EnableLocalization);
            }

            XElement HelpText = Document.Root.Element("HelpText");
            if (HelpText != null)
            {
                HelpText.Remove();
                rdlcSection.Add(HelpText);
            }

            XElement ReportDataSources = Document.Root.Element("ReportDataSources");
            if (ReportDataSources != null)
            {
                ReportDataSources.Remove();
                rdlcSection.Add(ReportDataSources);
            }

            XElement LoadDataOnPageLoad = Document.Root.Element("LoadDataOnPageLoad");
            if (LoadDataOnPageLoad != null)
            {
                LoadDataOnPageLoad.Remove();

                if (Document.Root.Element("Sections").Element("CriteriaSection") == null)
                {
                    LoadDataOnPageLoad.Value = "true";
                }

                rdlcSection.Add(LoadDataOnPageLoad);
            }

            XElement Parameters = Document.Root.Element("Parameters");
            if (Parameters != null)
            {
                Parameters.Remove();
                rdlcSection.Add(Parameters);
            }

            Document.Root.Element("Sections").Add(rdlcSection);

            XElement OnPageLoad = new XElement("OnPageLoad",
                        new XElement("Commands",
                            new XElement("DefaultOrPopulateScreenCommand",
                                new XElement("Name", "DefaultOrPopulateScreen"),
                                new XElement("DefaultCommand", DefaultCommand)
                                )
                            )
                        );

            if (Document.Root.Element("OnPageLoad") == null)
            {
                Document.Root.Add(OnPageLoad);
            }
        }

        private static void ProcessLogoutScreen(XDocument Document)
        {
            XElement editOnPageLoad = new XElement("OnPageLoad",
                        new XElement("Commands",
                            new XElement("LogoutCommand",
                                new XElement("Name", "Logout")
                                )
                            )
                        );

            if (Document.Root.Element("OnPageLoad") == null)
            {
                Document.Root.Add(editOnPageLoad);
            }
        }

        private static void ProcessRedirectScreen(XDocument Document)
        {
            XElement redirectSection = new XElement("RedirectCommand",
                new XElement("Name", "Redirect")
                );
            ;

            XElement DataCommand = Document.Root.Element("DataCommand");
            if (DataCommand != null)
            {
                DataCommand.Remove();
                redirectSection.Add(DataCommand);
            }

            XElement RedirectUrl = Document.Root.Element("RedirectUrl");
            if (RedirectUrl != null)
            {
                RedirectUrl.Remove();
                redirectSection.Add(RedirectUrl);
            }

            XElement RedirectUrlField = Document.Root.Element("RedirectUrlField");
            if (RedirectUrlField != null)
            {
                RedirectUrlField.Remove();
                redirectSection.Add(RedirectUrlField);
            }

            XElement RedirectMode = Document.Root.Element("RedirectMode");
            if (RedirectMode != null)
            {
                RedirectMode.Remove();
                redirectSection.Add(RedirectMode);
            }

            XElement onPageLoad = new XElement("OnPageLoad",
                        new XElement("Commands", redirectSection)
                        );

            if (Document.Root.Element("OnPageLoad") == null)
            {
                Document.Root.Add(onPageLoad);
            }
        }

        private static void ProcessReportExportScreen(XDocument Document)
        {
            XElement exportSection = new XElement("ExportRDLCCommand",
                new XElement("Name", "Export")
                );
            ;

            XElement ReportName = Document.Root.Element("ReportName");
            if (ReportName != null)
            {
                ReportName.Remove();
                exportSection.Add(ReportName);
            }

            XElement ExportFileName = Document.Root.Element("ExportFileName");
            if (ExportFileName != null)
            {
                ExportFileName.Remove();
                exportSection.Add(ExportFileName);
            }

            XElement EnableHyperlinks = Document.Root.Element("EnableHyperlinks");
            if (EnableHyperlinks != null)
            {
                EnableHyperlinks.Remove();
                exportSection.Add(EnableHyperlinks);
            }

            XElement ReportType = Document.Root.Element("ReportType");
            if (ReportType != null)
            {
                ReportType.Remove();
                exportSection.Add(ReportType);
            }

            XElement PageWidth = Document.Root.Element("PageWidth");
            if (PageWidth != null)
            {
                PageWidth.Remove();
                exportSection.Add(PageWidth);
            }

            XElement PageHeight = Document.Root.Element("PageHeight");
            if (PageHeight != null)
            {
                PageHeight.Remove();
                exportSection.Add(PageHeight);
            }

            XElement MarginTop = Document.Root.Element("MarginTop");
            if (MarginTop != null)
            {
                MarginTop.Remove();
                exportSection.Add(MarginTop);
            }

            XElement MarginLeft = Document.Root.Element("MarginLeft");
            if (MarginLeft != null)
            {
                MarginLeft.Remove();
                exportSection.Add(MarginLeft);
            }

            XElement MarginRight = Document.Root.Element("MarginRight");
            if (MarginRight != null)
            {
                MarginRight.Remove();
                exportSection.Add(MarginRight);
            }

            XElement MarginBottom = Document.Root.Element("MarginBottom");
            if (MarginBottom != null)
            {
                MarginBottom.Remove();
                exportSection.Add(MarginBottom);
            }

            XElement EnableLocalization = Document.Root.Element("EnableLocalization");
            if (EnableLocalization != null)
            {
                EnableLocalization.Remove();
                exportSection.Add(EnableLocalization);
            }

            XElement ReportDataSources = Document.Root.Element("ReportDataSources");
            if (ReportDataSources != null)
            {
                ReportDataSources.Remove();
                exportSection.Add(ReportDataSources);
            }

            XElement Parameters = Document.Root.Element("Parameters");
            if (Parameters != null)
            {
                Parameters.Remove();
                exportSection.Add(Parameters);
            }

            XElement onPageLoad = new XElement("OnPageLoad",
                        new XElement("Commands", exportSection)
                        );

            if (Document.Root.Element("OnPageLoad") == null)
            {
                Document.Root.Add(onPageLoad);
            }
        }

        private static void ProcessOverviewScreen(XDocument Document)
        {
            string EntityID = String.Empty;
            string EntityInputType = String.Empty;
            string RetrieveCommand = String.Empty;

            XElement EntityIDElement = Document.Root.Element("EntityID");
            if (EntityIDElement != null)
            {
                EntityID = EntityIDElement.Value;
                EntityIDElement.Remove();

            }


            XElement EntityInputTypeElement = Document.Root.Element("EntityInputType");
            if (EntityInputTypeElement != null)
            {
                EntityInputType = EntityInputTypeElement.Value;
                EntityInputTypeElement.Remove();

            }

            XElement RetrieveCommandElement = Document.Root.Element("RetrieveCommand");
            if (RetrieveCommandElement != null)
            {
                RetrieveCommand = RetrieveCommandElement.Value;
                RetrieveCommandElement.Remove();
            }

            XElement editOnPageLoad = new XElement("OnPageLoad",
                        new XElement("Commands",
                            new XElement("DefaultOrPopulateScreenCommand",
                                new XElement("Name", "DefaultOrPopulateScreen"),
                                new XElement("EntityID", EntityID),
                                new XElement("EntityInputType", EntityInputType),
                                new XElement("RetrieveCommand", RetrieveCommand)
                                )
                            )
                        );

            if (Document.Root.Element("OnPageLoad") == null)
            {
                Document.Root.Add(editOnPageLoad);
            }
        }

        private static void ProcessDocumentsScreen(XDocument Document)
        {
            string EntityID = String.Empty;
            string EntityInputType = String.Empty;
            string EntityType = String.Empty;

            XElement EntityIDElement = Document.Root.Element("EntityID");
            if (EntityIDElement != null)
            {
                EntityID = EntityIDElement.Value;
                EntityIDElement.Remove();

            }


            XElement EntityInputTypeElement = Document.Root.Element("EntityInputType");
            if (EntityInputTypeElement != null)
            {
                EntityInputType = EntityInputTypeElement.Value;
                EntityInputTypeElement.Remove();

            }

            XElement EntityTypeElement = Document.Root.Element("EntityType");
            if (EntityTypeElement != null)
            {
                EntityType = EntityTypeElement.Value;
                EntityTypeElement.Remove();

            }


            XElement DocumentTypes = Document.Root.Element("DocumentTypes");
            if (DocumentTypes != null)
            {
                DocumentTypes.Remove();
            }

            XElement DocumentTypeMode = Document.Root.Element("DocumentTypeMode");
            if (DocumentTypeMode != null)
            {
                DocumentTypeMode.Remove();
            }

            XElement DocumentListTypeMode = Document.Root.Element("DocumentListTypeMode");
            if (DocumentListTypeMode != null)
            {
                DocumentListTypeMode.Remove();
            }

            XElement DocumentTypeCommand = Document.Root.Element("DocumentTypeCommand");
            if (DocumentTypeCommand != null)
            {
                DocumentTypeCommand.Remove();
            }

            XElement DocumentsCommand = Document.Root.Element("DocumentsCommand");
            if (DocumentsCommand != null)
            {
                DocumentsCommand.Remove();
            }

            XElement DocumentTypeCodeField = Document.Root.Element("DocumentTypeCodeField");
            if (DocumentTypeCodeField != null)
            {
                DocumentTypeCodeField.Remove();
            }

            XElement DocumentTypeDisplayNameField = Document.Root.Element("DocumentTypeDisplayNameField");
            if (DocumentTypeDisplayNameField != null)
            {
                DocumentTypeDisplayNameField.Remove();
            }


            string AfterCancelConfirmationMessage = String.Empty;
            XElement AfterCancelConfirmationMessageElement = Document.Root.Element("AfterCancelConfirmationMessage");
            if (AfterCancelConfirmationMessageElement != null)
            {
                AfterCancelConfirmationMessage = AfterCancelConfirmationMessageElement.Value;
                AfterCancelConfirmationMessageElement.Remove();

            }


            string RedirectAfterCancel = String.Empty;
            XElement RedirectAfterCancelElement = Document.Root.Element("RedirectAfterCancel");
            if (RedirectAfterCancelElement != null)
            {
                RedirectAfterCancel = RedirectAfterCancelElement.Value;
                RedirectAfterCancelElement.Remove();

            }

            string AfterCancelRedirectUrl = String.Empty;
            XElement AfterCancelRedirectUrlElement = Document.Root.Element("AfterCancelRedirectUrl");
            if (AfterCancelRedirectUrlElement != null)
            {
                AfterCancelRedirectUrl = AfterCancelRedirectUrlElement.Value;
                AfterCancelRedirectUrlElement.Remove();

            }

            string AfterCancelRedirectUrlContext = String.Empty;
            XElement AfterCancelRedirectUrlContextElement = Document.Root.Element("AfterCancelRedirectUrlContext");
            if (AfterCancelRedirectUrlContextElement != null)
            {
                AfterCancelRedirectUrlContext = AfterCancelRedirectUrlContextElement.Value;
                AfterCancelRedirectUrlContextElement.Remove();

            }

            //create grid section
            XElement gridSection = new XElement("GridSection",
                new XElement("ID", "Existing_Documents_Section"),
                new XElement("Name", "Existing Documents"),
                new XElement("Type", "Grid"),
                new XElement("Mode", "All"),
                new XElement("Visible", true),
                new XElement("ContentPane", "Left"),
                new XElement("Permission",
                    new XElement("CheckPermission", false)
                ),

                new XElement("Grid",
                    new XElement("Name", "Existing Documents"),
                    new XElement("DataKeyNames", "DocumentID"),
                    new XElement("DataKeyParameterNames", "@DocumentID"),
                    new XElement("SelectDataCommand", "Document_GetDocumentsByEntityID"),
                    new XElement("AllowPaging", "true"),
                    new XElement("AllowDelete", "true"),
                    new XElement("DeleteDataCommand", "Document_Inactivate"),
                    new XElement("PageSize", "20"),
                    new XElement("AllowSorting", "true"),
                    new XElement("SortOrder", "Descending"),
                    new XElement("Columns",
                        new XElement("HyperLinkGridColumn",

                            new XElement("DataNavigateUrlFields", "DocumentID"),
                            new XElement("DataNavigateUrlFormatString", "~/App/Document/Download.aspx?DocumentID={0}"),
                            new XElement("Text", "Download"),
                            new XElement("Target", "_blank"),
                            new XElement("ColumnType", "HyperLinkGridColumn"),
                            new XElement("IncludeInExport", "false")
                        ),
                        new XElement("BoundGridColumn",
                            new XElement("HeaderText", "Name"),
                            new XElement("DataField", "DocumentName"),
                            new XElement("SortExpression", "DocumentName"),
                            new XElement("ColumnType", "BoundGridColumn"),
                            new XElement("IncludeInExport", "true")
                        ),
                        new XElement("BoundGridColumn",
                            new XElement("HeaderText", "Type"),
                            new XElement("DataField", "Description"),
                            new XElement("SortExpression", "Description"),
                            new XElement("ColumnType", "BoundGridColumn"),
                            new XElement("IncludeInExport", "true")
                        ),
                        new XElement("BoundGridColumn",
                            new XElement("HeaderText", "Uploaded On"),
                            new XElement("DataField", "CreatedOn"),
                            new XElement("SortExpression", "CreatedOn"),
                            new XElement("DataFormatString", "{0:MMM dd yyyy HH:MM:ss}"),
                            new XElement("ColumnType", "BoundGridColumn"),
                            new XElement("IncludeInExport", "true")
                        ),
                        new XElement("BoundGridColumn",
                            new XElement("HeaderText", "Uploaded By"),
                            new XElement("DataField", "FullName"),
                            new XElement("SortExpression", "FullName"),
                            new XElement("ColumnType", "BoundGridColumn"),
                            new XElement("IncludeInExport", "true")
                        ),

                        new XElement("DeleteGridColumn",
                            new XElement("Text", "Delete"),
                            new XElement("ConfirmText", "WARNING: You are about to delete this document\r\rPress OK to DELETE this document\rPress CANCEL to LEAVE this document alone"),
                            new XElement("ColumnType", "DeleteGridColumn"),
                            new XElement("ButtonType", "Link"),
                            new XElement("IncludeInExport", "false")
                        )
                    )
                )
           );

            Document.Root.Element("Sections").Add(gridSection);

            XElement fileUploadElement = new XElement("FileUploadControl",
                            new XElement("Name", "FilesToUpload"),
                            new XElement("Label", "Documents"),
                            new XElement("IsRequired", "true"),
                            new XElement("Visible", "true"),
                            new XElement("EntityID", EntityID),
                            new XElement("EntityInputType", EntityInputType),
                            new XElement("EntityType", EntityType),
                            new XElement("StorageMode", "Database"),
                            new XElement("MaxFileInputsCount", "0")
                            );





            //add edit section
            XElement EditSection = new XElement("EditSection",
                    new XElement("Name", "Select documents to upload here"),
                    new XElement("ID", "Documents_Upload_Section"),
                    new XElement("Type", "Edit"),
                    new XElement("Mode", "All"),
                    new XElement("Visible", true),
                    new XElement("ContentPane", "Left"),
                    new XElement("Permission",
                        new XElement("CheckPermission", false)
                    ),
                    new XElement("Controls")
                );

            if (
                (DocumentTypeMode == null) ||
                (
                    (DocumentTypeMode != null) &&
                    (DocumentTypeMode.Value.ToLower() == "static")
                )
             )
            {
                XElement docTypesLookupElement = new XElement("LookupDropDownListControl",
                           new XElement("Name", "DocType"),
                           new XElement("Type", "LookupDropDownList"),
                           new XElement("Label", "Document Type"),
                           new XElement("IsRequired", "true"),
                           new XElement("Visible", "true"),
                           new XElement("IncludeAdditionalListItem", "true"),
                           new XElement("AdditionalListItemText", "-- Select --"),
                           new XElement("LookupType", "DOCUMENT_TYPE")
                           );

                EditSection.Element("Controls").Add(docTypesLookupElement);
            }
            else
            {
                string DocumentTypeCommandValue = "";
                string DocumentTypeDisplayNameFieldValue = "";
                string DocumentTypeCodeFieldValue = "";

                if (DocumentTypeCommand != null)
                    DocumentTypeCommandValue = DocumentTypeCommand.Value;

                if (DocumentTypeDisplayNameField != null)
                    DocumentTypeDisplayNameFieldValue = DocumentTypeDisplayNameField.Value;

                if (DocumentTypeCodeField != null)
                    DocumentTypeCodeFieldValue = DocumentTypeCodeField.Value;

                XElement docTypesDropdownElement = new XElement("DropDownListControl",
                            new XElement("Name", "DocType"),
                            new XElement("Type", "DropDownList"),
                            new XElement("Label", "Document Type"),
                            new XElement("IsRequired", "true"),
                            new XElement("Visible", "true"),
                            new XElement("IncludeAdditionalListItem", "true"),
                            new XElement("AdditionalListItemText", "-- Select --"),
                            new XElement("SelectDataCommand", DocumentTypeCommandValue),
                            new XElement("DataTextField", DocumentTypeDisplayNameFieldValue),
                            new XElement("DataValueField", DocumentTypeCodeFieldValue)
                            );

                EditSection.Element("Controls").Add(docTypesDropdownElement);
            }


            EditSection.Element("Controls").Add(fileUploadElement);

            Document.Root.Element("Sections").Add(EditSection);


            //create button section
            XElement saveButton = new XElement("Button",
                    new XElement("Name", "Save"),
                    new XElement("Text", "Save"),
                    new XElement("CssClass", "btn btn-primary"),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "true"),
                    new XElement("OnClick",
                        new XElement("Commands",
                            new XElement("InsertUpdateSaveCommand",
                                new XElement("Name", "SaveCommand"),
                                new XElement("EntityID", EntityID),
                                new XElement("EntityInputType", EntityInputType),
                                new XElement("InsertCommand", "Document_UpdateEntity"),
                                new XElement("UpdateCommand", "Document_UpdateEntity"),
                                new XElement("AfterInsertConfirmationMessage", "Document uploaded"),
                                new XElement("AfterUpdateConfirmationMessage", "Document uploaded"),
                                new XElement("RedirectAfterInsert", "false"),
                                new XElement("RedirectAfterUpdate", "false")
                                ),

                            new XElement("SetControlPropertyCommand",
                                new XElement("Name", "ClearFiles"),
                                new XElement("ControlName", "FilesToUpload"),
                                new XElement("PropertyName", "Value"),
                                new XElement("PropertyValue", "")
                                )
                            )
                        )
                    );



            XElement cancelButton = new XElement("Button",
                    new XElement("Name", "Cancel"),
                    new XElement("Text", "Cancel"),
                    new XElement("CssClass", "btn btn-default"),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "false"),
                    new XElement("OnClick",
                        new XElement("Commands",
                            new XElement("NavigateToUrlCommand",
                                new XElement("Name", "NavigateOnCancel"),
                                new XElement("EntityID", EntityID),
                                new XElement("NavigateUrl", AfterCancelRedirectUrl),
                                new XElement("Context", AfterCancelRedirectUrlContext)
                                )
                            )
                        )
                    );



            XElement buttonSection = new XElement("ButtonListSection",
                new XElement("Name", "Buttons"),
                new XElement("Type", "ButtonList"),
                new XElement("CssClass", "text-center"),
                new XElement("ContainerMode", "Plain"),
                new XElement("Mode", "All"),
                new XElement("Visible", true),
                new XElement("ContentPane", "Bottom"),
                new XElement("Permission",
                    new XElement("CheckPermission", false)
                ),
                new XElement("Buttons",
                    new XElement("CheckPermission", false),
                    saveButton,
                    cancelButton
                )
            );

            Document.Root.Element("Sections").Add(buttonSection);



            if (Document.Root.Element("DataCommands") == null)
            {
                Document.Root.Add(new XElement("DataCommands"));
            }

            XElement document_GetDocumentsByEntityID_dataCommand = new XElement("DataCommand",
                  new XElement("Name", "Document_GetDocumentsByEntityID"),
                  new XElement("Parameters",
                        new XElement("Parameter",
                                new XElement("Name", "@EntityID"),
                                new XElement("InputType", EntityInputType),
                                new XElement("InputKey", EntityID)
                            ),
                        new XElement("Parameter",
                                new XElement("Name", "@EntityType"),
                                new XElement("InputType", "Constant"),
                                new XElement("InputKey", EntityType)
                            )
                      )
                );

            XElement document_Inactivate_dataCommand = new XElement("DataCommand",
                  new XElement("Name", "Document_Inactivate"),
                  new XElement("Parameters",
                        new XElement("Parameter",
                                new XElement("Name", "@DocumentID"),
                                new XElement("InputType", "Special"),
                                new XElement("InputKey", "Null")
                            ),
                        new XElement("Parameter",
                                new XElement("Name", "@ModifiedBy"),
                                new XElement("InputType", "Special"),
                                new XElement("InputKey", "UserName")
                            )
                      )
                );

            XElement document_UpdateEntity_dataCommand = new XElement("DataCommand",
                  new XElement("Name", "Document_UpdateEntity"),
                  new XElement("Parameters",
                      new XElement("Parameter",
                                new XElement("Name", "@DocumentID"),
                                new XElement("InputType", "Control"),
                                new XElement("InputKey", "FilesToUpload")
                            ),
                      new XElement("Parameter",
                                new XElement("Name", "@EntityID"),
                                new XElement("InputType", EntityInputType),
                                new XElement("InputKey", EntityID)
                            ),
                        new XElement("Parameter",
                                new XElement("Name", "@EntityType"),
                                new XElement("InputType", "Constant"),
                                new XElement("InputKey", EntityType)
                            ),

                        new XElement("Parameter",
                                new XElement("Name", "@ModifiedBy"),
                                new XElement("InputType", "Special"),
                                new XElement("InputKey", "UserName")
                            )

                      )
                );

            Document.Root.Element("DataCommands").Add(document_GetDocumentsByEntityID_dataCommand);
            Document.Root.Element("DataCommands").Add(document_UpdateEntity_dataCommand);
            Document.Root.Element("DataCommands").Add(document_Inactivate_dataCommand);


            AddInsertEditRenderPageSectionsCommand(Document, EntityID, EntityInputType);
        }

        private static void ProcessContentScreen(XDocument Document)
        {
            XElement templateSection = new XElement("TemplateSection",
                    new XElement("Type", "Template"),
                    new XElement("Mode", "All"),
                    new XElement("Visible", true),
                    new XElement("ContentPane", "Left"),
                    new XElement("Permission",
                        new XElement("CheckPermission", false)
                    )
                );

            XElement ContentSelectionMode = Document.Root.Element("ContentSelectionMode");
            if (ContentSelectionMode != null)
            {
                ContentSelectionMode.Remove();
                templateSection.Add(ContentSelectionMode);
            }

            XElement TemplateSelectionMode = Document.Root.Element("TemplateSelectionMode");
            if (TemplateSelectionMode != null)
            {
                TemplateSelectionMode.Remove();
                templateSection.Add(TemplateSelectionMode);
            }

            XElement ContentTemplate = Document.Root.Element("ContentTemplate");
            if (ContentTemplate != null)
            {
                ContentTemplate.Remove();
                templateSection.Add(ContentTemplate);
            }

            XElement ContentTemplateDataCommand = Document.Root.Element("ContentTemplateDataCommand");
            if (ContentTemplateDataCommand != null)
            {
                ContentTemplateDataCommand.Remove();
                templateSection.Add(ContentTemplateDataCommand);
            }

            XElement ContentTemplateField = Document.Root.Element("ContentTemplateField");
            if (ContentTemplateField != null)
            {
                ContentTemplateField.Remove();
                templateSection.Add(ContentTemplateField);
            }

            XElement ContentField = Document.Root.Element("ContentField");
            if (ContentField != null)
            {
                ContentField.Remove();
                templateSection.Add(ContentField);
            }

            XElement DataItems = Document.Root.Element("DataItems");
            if (DataItems != null)
            {
                DataItems.Remove();
                templateSection.Add(DataItems);
            }

            Document.Root.Element("Sections").Add(templateSection);
        }



        private static void AddSimpleRenderPageSectionsCommand(XDocument Document)
        {
            XElement RenderPageSections = null;

            if (Document.Root.Element("OnPageInit") == null)
            {
                Document.Root.Add(
                       new XElement("OnPageInit")
                   );


            }

            if (Document.Root.Element("OnPageInit").Element("Commands") == null)
            {
                Document.Root.Element("OnPageInit").Add(
                       new XElement("Commands")
                   );

            }

            RenderPageSections = new XElement("RenderPageSectionsCommand",
                                        new XElement("Name", "RenderPageSections"),
                                        new XElement("Mode", "Simple")
                                        );


            Document.Root.Element("OnPageInit").Element("Commands").Add(RenderPageSections);
        }

        private static void AddInsertEditRenderPageSectionsCommand(XDocument Document, string EntityID, string EntityInputType)
        {
            XElement RenderPageSections = null;

            if (Document.Root.Element("OnPageInit") == null)
            {
                Document.Root.Add(
                       new XElement("OnPageInit")
                   );


            }

            if (Document.Root.Element("OnPageInit").Element("Commands") == null)
            {
                Document.Root.Element("OnPageInit").Add(
                       new XElement("Commands")
                   );

            }

            RenderPageSections = new XElement("RenderPageSectionsCommand",
                                        new XElement("Name", "RenderPageSections"),
                                        new XElement("Mode", "InsertEdit"),
                                        new XElement("EntityID", EntityID),
                                        new XElement("EntityInputType", EntityInputType)
                                        );


            Document.Root.Element("OnPageInit").Element("Commands").Add(RenderPageSections);


        }

        private static void ConvertGridToGridSection(XDocument Document)
        {
            XElement grid = Document.Root.Element("Grid");
            if (grid != null)
            {
                string SelectDataCommand = null;

                //<LoadDataOnPageLoad>false</LoadDataOnPageLoad>
                XElement LoadDataOnPageLoad = null;
                if (Document.Root.Element("LoadDataOnPageLoad") != null)
                {
                    LoadDataOnPageLoad = Document.Root.Element("LoadDataOnPageLoad");
                    LoadDataOnPageLoad.Remove();
                }
                else
                {
                    LoadDataOnPageLoad = new XElement("LoadDataOnPageLoad", true);
                }


                if (grid.Element("SelectDataCommand") != null)
                {
                    SelectDataCommand = grid.Element("SelectDataCommand").Value;
                }

                string HelpText = String.Empty;
                if (grid.Element("HelpText") != null)
                {
                    HelpText = grid.Element("HelpText").Value;
                    grid.Element("HelpText").Remove();
                }

                grid.Remove();

                //elements to modify


                XElement gridSection = new XElement("GridSection",
                    new XElement("Name", grid.Name),
                    new XElement("Type", "Grid"),
                    new XElement("IntroText", HelpText),
                    new XElement("Mode", "All"),
                    new XElement("Visible", true),
                    new XElement("ContentPane", "Left"),
                    new XElement("SelectDataCommand", SelectDataCommand),
                    new XElement("Permission",
                        new XElement("CheckPermission", false)
                    ),
                    new XElement("ActionLink",
                        new XElement("ShowLink", false),
                        new XElement("Permission",
                            new XElement("CheckPermission", false)
                        )
                    ),
                    LoadDataOnPageLoad,
                    grid
                );

                Document.Root.Element("Sections").Add(gridSection);
            }
        }

        private static void ConvertGridToEditableGridSection(XDocument Document)
        {
            XElement grid = Document.Root.Element("Grid");
            if (grid != null)
            {
                string SelectDataCommand = null;

                //<LoadDataOnPageLoad>false</LoadDataOnPageLoad>
                XElement LoadDataOnPageLoad = null;
                if (Document.Root.Element("LoadDataOnPageLoad") != null)
                {
                    LoadDataOnPageLoad = Document.Root.Element("LoadDataOnPageLoad");
                    LoadDataOnPageLoad.Remove();
                }
                else
                {
                    LoadDataOnPageLoad = new XElement("LoadDataOnPageLoad", true);
                }


                if (grid.Element("SelectDataCommand") != null)
                {
                    SelectDataCommand = grid.Element("SelectDataCommand").Value;
                }

                string HelpText = String.Empty;
                if (grid.Element("HelpText") != null)
                {
                    HelpText = grid.Element("HelpText").Value;
                    grid.Element("HelpText").Remove();
                }

                string InsertCommand = String.Empty;
                XElement InsertCommandElement = Document.Root.Element("InsertCommand");
                if (InsertCommandElement != null)
                {
                    InsertCommand = InsertCommandElement.Value;
                    InsertCommandElement.Remove();

                }

                string UpdateCommand = String.Empty;
                XElement UpdateCommandElement = Document.Root.Element("UpdateCommand");
                if (UpdateCommandElement != null)
                {
                    UpdateCommand = UpdateCommandElement.Value;
                    UpdateCommandElement.Remove();

                }

                bool UseDefaultCommand = false;
                XElement UseDefaultCommandElement = Document.Root.Element("UseDefaultCommand");
                if (UseDefaultCommandElement != null)
                {
                    UseDefaultCommand = bool.Parse(UseDefaultCommandElement.Value);
                    UseDefaultCommandElement.Remove();
                }

                string DefaultCommand = String.Empty;
                XElement DefaultCommandElement = Document.Root.Element("DefaultCommand");
                if (DefaultCommandElement != null)
                {
                    DefaultCommand = DefaultCommandElement.Value;
                    DefaultCommandElement.Remove();
                }

                if (!UseDefaultCommand)
                    DefaultCommand = String.Empty;

                grid.Remove();


                XElement SectionsElement = Document.Root.Element("Sections");
                if (SectionsElement != null)
                {
                    SectionsElement.Remove();
                }

                XElement SectionZoneLayoutElement = Document.Root.Element("SectionZoneLayout");
                if (SectionZoneLayoutElement != null)
                {

                    SectionZoneLayoutElement.Remove();
                }

                XElement ActionLinkElement = Document.Root.Element("ActionLink");
                if (ActionLinkElement != null)
                {
                    ActionLinkElement.Remove();
                }


                XElement saveButton = new XElement("Button",
                    new XElement("Name", "Save"),
                    new XElement("CssClass", "btn btn-primary"),
                    new XElement("Text", "Save"),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "true"),
                    new XElement("CommandName", "PerformInsert")

                );


                XElement cancelButton = new XElement("Button",
                    new XElement("Name", "Cancel"),
                    new XElement("CssClass", "btn btn-default"),
                    new XElement("Text", "Cancel"),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "false"),
                    new XElement("CommandName", "Cancel")

                );

                XElement buttonSection = new XElement("ButtonListSection",
                    new XElement("Name", "Buttons"),
                    new XElement("Type", "ButtonList"),
                    new XElement("CssClass", "text-center"),
                    new XElement("ContainerMode", "Plain"),
                    new XElement("Mode", "All"),
                    new XElement("Visible", true),
                    new XElement("ContentPane", "Bottom"),
                    new XElement("Permission",
                        new XElement("CheckPermission", false)
                    ),
                    new XElement("Buttons",
                        new XElement("CheckPermission", false),
                        saveButton,
                        cancelButton
                    )
                );

                SectionsElement.Add(buttonSection);

                XElement gridSection = new XElement("EditableGridSection",
                    new XElement("Name", grid.Name),
                    new XElement("Type", "EditableGrid"),
                    new XElement("IntroText", HelpText),
                    new XElement("Mode", "All"),
                    new XElement("Visible", true),
                    new XElement("ContentPane", "Left"),
                    new XElement("SelectDataCommand", SelectDataCommand),
                    new XElement("InsertCommand", InsertCommand),
                    new XElement("UpdateCommand", UpdateCommand),
                    new XElement("DefaultCommand", DefaultCommand),
                    new XElement("Permission",
                        new XElement("CheckPermission", false)
                    ),
                    ActionLinkElement,
                    LoadDataOnPageLoad,
                    grid,
                    SectionsElement,
                    SectionZoneLayoutElement
                );

                CreateSections(Document);

                Document.Root.Element("Sections").Add(gridSection);


            }
        }

        private static void ConvertCriteriaToCriteriaSection(XDocument Document, bool AddFirst)
        {
            XElement criteria = Document.Root.Element("Criteria");
            if (criteria != null)
            {
                criteria.Remove();
                criteria.Name = "CriteriaSection";

                string HelpText = String.Empty;
                if (criteria.Element("HelpText") != null)
                {
                    criteria.Element("HelpText").Name = "IntroText";
                }

                string ActionButtonText = "Search";
                if (criteria.Element("ActionButtonText") != null)
                {
                    ActionButtonText = criteria.Element("ActionButtonText").Value;
                    criteria.Element("ActionButtonText").Remove();

                }
                

                int ControlsPerRow = 1;
                if (criteria.Element("ControlsPerRow") != null)
                {
                    if(!Int32.TryParse(criteria.Element("ControlsPerRow").Value, out ControlsPerRow))
                        ControlsPerRow = 1; 
                }

                int columnWidth = (12 / ControlsPerRow);
                if (columnWidth < 1)
                    columnWidth = 1;
                if (columnWidth > 12)
                    columnWidth = 12;

                //elements to modify
                XElement SectionName = criteria.Element("SectionHeader");
                if (SectionName != null)
                    SectionName.Name = "Name";

                //elements to add
                criteria.AddFirst(
                       new XElement("Type", "Criteria")
                   );

                criteria.Add(
                        new XElement("Mode", "All")
                    );

                criteria.Add(
                        new XElement("Visible", "true")
                    );

                criteria.Add(
                        new XElement("ContentPane", "Left")
                    );

                criteria.Add(
                        new XElement("ColumnElement", "div")
                    );

                criteria.Add(
                        new XElement("ColumnCssClass", String.Format("col-md-{0}", columnWidth))
                    );

                criteria.Add(
                        new XElement("RowElement", "div")
                    );

                criteria.Add(
                        new XElement("RowCssClass", "row")
                    );

                bool hasControls = false;
                if (criteria.Element("Controls") != null)
                {
                    hasControls = criteria.Element("Controls").HasElements;
                }

                if (hasControls)
                {
                    XElement criteriaButton = new XElement(
                            "ButtonControl",
                                new XElement("Name", "Search"),
                                new XElement("Type", "Button"),
                                new XElement("Text", ActionButtonText),
                                new XElement("CssClass", "btn btn-primary criteria-button"),
                                new XElement("Visible", "true"),
                                new XElement("IsRequired", "false"),
                                new XElement("ControlGroupElement", "div"),
                                new XElement("ControlGroupCssClass", "form-group"),
                                new XElement("CausesValidation", "false"),
                                new XElement("OnClick",
                                    new XElement("Commands", 
                                        new XElement("InvokeSearchMessageCommand", 
                                            new XElement("Name", "CriteriaClick")
                                        )
                                    )
                               )
                        );

                    criteria.Element("Controls").Add(criteriaButton);

                    if (AddFirst)
                        Document.Root.Element("Sections").AddFirst(criteria);
                    else
                        Document.Root.Element("Sections").Add(criteria);
                }


            }
        }

        private static void ConvertAfterPopulateScreenToPageLoad(XDocument Document)
        {
            XElement AfterPopulateScreen = Document.Root.Element("AfterPopulateScreen");
            if (AfterPopulateScreen != null)
            {
                AfterPopulateScreen.Remove();

                if (AfterPopulateScreen.Element("Commands") != null)
                {
                    if (AfterPopulateScreen.Element("Commands").HasElements)
                    {
                        foreach (XElement command in AfterPopulateScreen.Element("Commands").Elements())
                        {
                            XElement c = ConvertCommandToNewFormat(command);

                            //add the command to page load
                            if (Document.Root.Element("OnPageLoad") == null)
                            {
                                Document.Root.Add(new XElement("OnPageLoad"));
                            }

                            if (Document.Root.Element("OnPageLoad").Element("Commands") == null)
                            {
                                Document.Root.Element("OnPageLoad").Add(new XElement("Commands"));
                            }

                            Document.Root.Element("OnPageLoad").Element("Commands").Add(c);

                        }
                    }
                }
            }
        }

        private static void ConvertSectionAndControlActions(XDocument Document)
        {
            XElement Sections = Document.Root.Element("Sections");
            if (Sections != null)
            {
                foreach (XElement section in Sections.Elements())
                {
                    ConvertSectionControlsActions(section);

                    if (section.Name.LocalName == "EditableGridSection")
                    {
                        XElement gridSections = section.Element("Sections");
                        if (gridSections != null)
                        {
                            foreach (XElement gridSection in gridSections.Elements())
                            {
                                ConvertSectionControlsActions( gridSection);

                                

                                //move after populate section to page load
                                ConvertAfterPopulateSection(Document, gridSection);

                                //process section controls
                                if (gridSection.Element("Controls") != null)
                                {
                                    foreach (XElement control in gridSection.Element("Controls").Elements())
                                    {
                                        ConvertControl(Document, gridSection, control);
                                    }
                                }
                            }
                        }
                    }

                    //move after populate section to page load
                    ConvertAfterPopulateSection(Document, section);

                    //process section controls
                    if (section.Element("Controls") != null)
                    {
                        foreach (XElement control in section.Element("Controls").Elements())
                        {
                            ConvertControl(Document, section, control);
                        }
                    }
                }
            }
            
        }

        private static void ConvertControl(XDocument Document, XElement section, XElement control)
        {
            String LabelRenderingMode = "Left";

            XElement LabelRenderingModeElement = control.Element("LabelRenderingMode");
            if (LabelRenderingModeElement != null)
            {
                LabelRenderingMode = LabelRenderingModeElement.Value;
                LabelRenderingModeElement.Remove();
            }

            XElement WidthElement = control.Element("Width");
            if (WidthElement != null)
            {
                WidthElement.Remove();
            }

            string SectionZoneLayout = "Default";
            XElement SectionZoneLayoutElement = Document.Root.Element("SectionZoneLayout");
            if (SectionZoneLayoutElement != null)
            {
                SectionZoneLayout = SectionZoneLayoutElement.Value;
            }

            if (section.Name.LocalName != "CriteriaSection")
            {
                //at present time - it should be details or edit section 

                if (section.Element("ContainerElement") == null)
                {
                    section.Add(
                        new XElement("ContainerElement", "fieldset"),
                        new XElement("ContainerCssClass", "form-horizontal")
                    );
                }


                control.Add(
                    new XElement("ControlGroupElement", "div"),
                    new XElement("ControlGroupCssClass", "form-group"),
                    new XElement("LabelRendersBeforeControl", "true"),
                    new XElement("LabelWrapsControl", "false"),
                    new XElement("ControlContainerElement", "div"),
                    new XElement("HelpTextElement", "span"),
                    new XElement("HelpTextCssClass", "help-block")
               );

                switch(LabelRenderingMode.ToLower())
                    {
                        case "left":
                            if (SectionZoneLayout.ToLower() == "default")
                            {
                                control.Add(
                                    new XElement("LabelCssClass", "col-md-4"),
                                    new XElement("ControlContainerCssClass", "col-md-8")
                                );
                            }
                            else
                            {
                                control.Add(
                                        new XElement("LabelCssClass", "col-md-2"),
                                        new XElement("ControlContainerCssClass", "col-md-10")
                                    );
                            }
                            break;
                        default:
                            control.Add(
                                new XElement("LabelCssClass", "col-md-12"),
                                new XElement("ControlContainerCssClass", "col-md-12")
                            );
                            break;
                    }
            }
            else
            {
                control.Add(
                    new XElement("ControlGroupElement", "div"),
                    new XElement("ControlGroupCssClass", "form-group"),

                    new XElement("LabelRendersBeforeControl", "true"),
                    new XElement("LabelWrapsControl", "false"),

                    new XElement("HelpTextElement", "span"),
                    new XElement("HelpTextCssClass", "help-block")
               );
            }
        }

        private static void ConvertAfterPopulateSection(XDocument Document, XElement section)
        {
            XElement AfterPopulateSection = section.Element("AfterPopulateSection");
            if (AfterPopulateSection != null)
            {
                //AfterPopulateSection.Remove();

                if (AfterPopulateSection.Element("Commands") != null)
                {
                    if (AfterPopulateSection.Element("Commands").HasElements)
                    {
                        foreach (XElement command in AfterPopulateSection.Element("Commands").Elements())
                        {
                            XElement c = ConvertCommandToNewFormat(command);

                            command.Remove();
                            AfterPopulateSection.Element("Commands").Add(c);

                            

                        }
                    }
                }
            }
        }

        private static void ConvertSectionControlsActions( XElement section)
        {
            if ((section.Element("Controls") != null) && (section.Element("Controls").HasElements))
            {
                foreach (XElement control in section.Element("Controls").Elements())
                {
                    if (
                        (control.Name.LocalName == "DropDownListControl") ||
                        (control.Name.LocalName == "LookupDropDownListControl")
                        )
                    {
                        XElement SelectedIndexChanged = control.Element("SelectedIndexChanged");
                        if (SelectedIndexChanged != null)
                        {


                            if (SelectedIndexChanged.Element("Commands") != null)
                            {
                                if (SelectedIndexChanged.Element("Commands").HasElements)
                                {
                                    foreach (XElement command in SelectedIndexChanged.Element("Commands").Elements())
                                    {
                                        command.Remove();
                                        XElement c = ConvertCommandToNewFormat(command);

                                        //add the command to page load
                                        SelectedIndexChanged.Element("Commands").Add(c);

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static XElement ConvertCommandToNewFormat(XElement command)
        {
            string CommandName = String.Empty;
            string DataCommandName = String.Empty;

            if (command.Element("Command") != null)
                DataCommandName = command.Element("Command").Value;

            if (command.Element("Name") != null)
                CommandName = command.Element("Name").Value;

            XElement c = null;
            if (command.Element("CompletionRoutine") != null)
            {
                //create SetScreenObjects command
                c = new XElement("SetScreenObjectsDataCommand",
                        new XElement("Name", CommandName),
                        new XElement("Type", "SetScreenObjectsDataCommand"),
                        new XElement("DataCommand", DataCommandName)
                    );
            }
            else
            {
                //create Execute datacommand task
                c = new XElement("ExecuteDataCommand",
                        new XElement("Name", CommandName),
                        new XElement("Type", "ExecuteDataCommand"),
                        new XElement("DataCommand", DataCommandName)
                    );
            }
            return c;
        }

        private static void ConvertActionLinksToLinksSection(XDocument Document)
        {
            XElement actionLink = Document.Root.Element("ActionLink");
            if (actionLink != null)
            {
                actionLink.Remove();
                actionLink.Name = "Item";

                XElement ShowLink = actionLink.Element("ShowLink");
                if (ShowLink != null)
                {
                    ShowLink.Name = "Visible";

                    XElement text = actionLink.Element("Text");
                    if (text != null)
                    {
                        text.Value = "<span class='glyphicon glyphicon-plus'></span> " + text.Value;

                        
                        
                    }

                    XElement actionLinkPermission = actionLink.Element("Permission");
                    XElement actionLinksSection = new XElement("LinkListSection");

                    //elements to add
                    actionLinksSection.Add(
                           new XElement("Type", "LinkList"),
                           new XElement("Mode", "All"),
                           new XElement("ContainerElement", "ul"),
                           new XElement("ItemElement", "li"),
                           new XElement("Mode", "All"),
                           new XElement("ContainerMode", "Plain"),
                           new XElement("CssClass", "section-links"),
                           new XElement("Visible", "true"),
                           new XElement("ContentPane", "Top"),
                           new XElement("Items", actionLink)
                       );

                   

                    if (actionLinkPermission != null)
                    {
                        actionLinkPermission.Remove();
                        actionLinksSection.Add(actionLinkPermission);
                    }

                    if (ShowLink.Value.ToLower() == "true")
                    {
                        Document.Root.Element("Sections").Add(actionLinksSection);
                    }
                }


            }
        }

        private static void CreateAlertsSection(XDocument Document, bool IncludeValidationSummary)
        {
            if (Document.Root.Element("Sections").Element("AlertSection") == null)
            {
                Document.Root.Element("Sections").AddFirst(
                        new XElement("AlertSection",
                            new XElement("Name", "Alert"),
                            new XElement("Type", "Alert"),
                            new XElement("Mode", "All"),
                            new XElement("ContainerMode", "None"),
                            new XElement("Visible", true),
                            new XElement("ContentPane", "Top"),
                            new XElement("IncludeValidationSummary", IncludeValidationSummary),
                            new XElement("Permission",
                                new XElement("CheckPermission", false)
                                )
                           )
                        );

            }
        }

        private static void CreateSections(XDocument Document)
        {
            //Add Sections node if it does not exist
            if (Document.Root.Element("Sections") == null)
            {
                Document.Root.Add(
                        new XElement("Sections")
                    );
            }
            //SectionZoneLayout

            if (Document.Root.Element("SectionZoneLayout") == null)
            {
                string Zone = "SingleColumn";

                switch (Document.Root.Name.LocalName.ToLower())
                {
                    case "editscreen":
                        Zone = "Default";
                        break;
                    case "overviewscreen":
                        Zone = "Default";
                        break;
                   
                    default:
                        Zone = "SingleColumn";
                        break;
                }

                Document.Root.Add(
                        new XElement("SectionZoneLayout", Zone)
                    );
            }
        }

        private static void ProcessEditScreen(XDocument Document)
        {
            //save
            string EntityID = String.Empty;
            string EntityInputType = String.Empty;
            string SaveButtonName = "Save";

            XElement buttonName = Document.Root.Element("ButtonName");
            if (buttonName != null)
            {
                SaveButtonName = buttonName.Value;
                buttonName.Remove();

            }


            XElement EntityIDElement = Document.Root.Element("EntityID");
            if (EntityIDElement != null)
            {
                EntityID = EntityIDElement.Value;
                EntityIDElement.Remove();

            }


            XElement EntityInputTypeElement = Document.Root.Element("EntityInputType");
            if (EntityInputTypeElement != null)
            {
                EntityInputType = EntityInputTypeElement.Value;
                EntityInputTypeElement.Remove();

            }

            string InsertCommand = String.Empty;
            XElement InsertCommandElement = Document.Root.Element("InsertCommand");
            if (InsertCommandElement != null)
            {
                InsertCommand = InsertCommandElement.Value;
                InsertCommandElement.Remove();

            }

            string UpdateCommand = String.Empty;
            XElement UpdateCommandElement = Document.Root.Element("UpdateCommand");
            if (UpdateCommandElement != null)
            {
                UpdateCommand = UpdateCommandElement.Value;
                UpdateCommandElement.Remove();

            }

            string AfterInsertConfirmationMessage = String.Empty;
            string AfterUpdateConfirmationMessage = String.Empty;
            string RedirectAfterInsert = String.Empty;
            string RedirectAfterUpdate = String.Empty;
            string AfterUpdateRedirectUrl = String.Empty;
            string AfterInsertRedirectUrl = String.Empty;
            string AfterUpdateRedirectUrlContext = String.Empty;
            string AfterInsertRedirectUrlContext = String.Empty;

            string AfterCancelConfirmationMessage = String.Empty;
            string RedirectAfterCancel = String.Empty;
            string AfterCancelRedirectUrl = String.Empty;
            string AfterCancelRedirectUrlContext = String.Empty;


            XElement SaveActionElement = Document.Root.Element("SaveAction");
            if (SaveActionElement != null)
            {



                XElement AfterInsertConfirmationMessageElement = SaveActionElement.Element("AfterInsertConfirmationMessage");
                if (AfterInsertConfirmationMessageElement != null)
                {
                    AfterInsertConfirmationMessage = AfterInsertConfirmationMessageElement.Value;
                    AfterInsertConfirmationMessageElement.Remove();

                }


                XElement AfterUpdateConfirmationMessageElement = SaveActionElement.Element("AfterUpdateConfirmationMessage");
                if (AfterUpdateConfirmationMessageElement != null)
                {
                    AfterUpdateConfirmationMessage = AfterUpdateConfirmationMessageElement.Value;
                    AfterUpdateConfirmationMessageElement.Remove();

                }

                XElement AfterCancelConfirmationMessageElement = SaveActionElement.Element("AfterCancelConfirmationMessage");
                if (AfterCancelConfirmationMessageElement != null)
                {
                    AfterCancelConfirmationMessage = AfterCancelConfirmationMessageElement.Value;
                    AfterCancelConfirmationMessageElement.Remove();

                }


                XElement RedirectAfterInsertMessageElement = SaveActionElement.Element("RedirectAfterInsert");
                if (RedirectAfterInsertMessageElement != null)
                {
                    RedirectAfterInsert = RedirectAfterInsertMessageElement.Value;
                    RedirectAfterInsertMessageElement.Remove();

                }


                XElement RedirectAfterUpdateElement = SaveActionElement.Element("RedirectAfterUpdate");
                if (RedirectAfterUpdateElement != null)
                {
                    RedirectAfterUpdate = RedirectAfterUpdateElement.Value;
                    RedirectAfterUpdateElement.Remove();

                }

                XElement RedirectAfterCancelElement = SaveActionElement.Element("RedirectAfterCancel");
                if (RedirectAfterCancelElement != null)
                {
                    RedirectAfterCancel = RedirectAfterCancelElement.Value;
                    RedirectAfterCancelElement.Remove();

                }


                XElement AfterUpdateRedirectUrlElement = SaveActionElement.Element("AfterUpdateRedirectUrl");
                if (AfterUpdateRedirectUrlElement != null)
                {
                    AfterUpdateRedirectUrl = AfterUpdateRedirectUrlElement.Value;
                    AfterUpdateRedirectUrlElement.Remove();

                }


                XElement AfterInsertRedirectUrlElement = SaveActionElement.Element("AfterInsertRedirectUrl");
                if (AfterInsertRedirectUrlElement != null)
                {
                    AfterInsertRedirectUrl = AfterInsertRedirectUrlElement.Value;
                    AfterInsertRedirectUrlElement.Remove();

                }

                XElement AfterCancelRedirectUrlElement = SaveActionElement.Element("AfterCancelRedirectUrl");
                if (AfterCancelRedirectUrlElement != null)
                {
                    AfterCancelRedirectUrl = AfterCancelRedirectUrlElement.Value;
                    AfterCancelRedirectUrlElement.Remove();

                }


                XElement AfterUpdateRedirectUrlContextElement = SaveActionElement.Element("AfterUpdateRedirectUrlContext");
                if (AfterUpdateRedirectUrlContextElement != null)
                {
                    AfterUpdateRedirectUrlContext = AfterUpdateRedirectUrlContextElement.Value;
                    AfterUpdateRedirectUrlContextElement.Remove();

                }


                XElement AfterInsertRedirectUrlContextElement = SaveActionElement.Element("AfterInsertRedirectUrlContext");
                if (AfterInsertRedirectUrlContextElement != null)
                {
                    AfterInsertRedirectUrlContext = AfterInsertRedirectUrlContextElement.Value;
                    AfterInsertRedirectUrlContextElement.Remove();

                }

                XElement AfterCancelRedirectUrlContextElement = SaveActionElement.Element("AfterCancelRedirectUrlContext");
                if (AfterCancelRedirectUrlContextElement != null)
                {
                    AfterCancelRedirectUrlContext = AfterCancelRedirectUrlContextElement.Value;
                    AfterCancelRedirectUrlContextElement.Remove();

                }
            }

            string SaveButtonOnClick = String.Empty;
            XElement SaveButtonOnClientClickElement = Document.Root.Element("SaveButtonOnClientClick");
            if (SaveButtonOnClientClickElement != null)
            {
                SaveButtonOnClick = SaveButtonOnClientClickElement.Value;

                SaveButtonOnClientClickElement.Remove();
            }

            XElement saveButton = new XElement("Button",
                    new XElement("Name", "Save"),
                    new XElement("CssClass", "btn btn-primary"),
                //ContainerMode
                    new XElement("Text", SaveButtonName),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "true"),
                    new XElement("OnClientClick", SaveButtonOnClick),
                    new XElement("OnClick",
                        new XElement("Commands",
                            new XElement("InsertUpdateSaveCommand",
                                new XElement("Name", "SaveCommand"),
                                new XElement("EntityID", EntityID),
                                new XElement("EntityInputType", EntityInputType),
                                new XElement("InsertCommand", InsertCommand),
                                new XElement("UpdateCommand", UpdateCommand),
                                new XElement("AfterInsertConfirmationMessage", AfterInsertConfirmationMessage),
                                new XElement("AfterUpdateConfirmationMessage", AfterUpdateConfirmationMessage),
                                new XElement("RedirectAfterInsert", RedirectAfterInsert),
                                new XElement("RedirectAfterUpdate", RedirectAfterUpdate),
                                new XElement("AfterUpdateRedirectUrl", AfterUpdateRedirectUrl),
                                new XElement("AfterInsertRedirectUrl", AfterInsertRedirectUrl),
                                new XElement("AfterUpdateRedirectUrlContext", AfterUpdateRedirectUrlContext),
                                new XElement("AfterInsertRedirectUrlContext", AfterInsertRedirectUrlContext)
                                )
                            )
                        )
                );



            //next
            XElement nextButton = null;
            bool EnableNextButton = false;
            string NextButtonName = "Next";
            string NextButtonRedirectUrl = null;
            string NextButtonRedirectUrlContext = null;
            bool SaveOnNext = false;

            XElement EnableNextButtonElement = Document.Root.Element("EnableNextButton");
            if (EnableNextButtonElement != null)
            {
                EnableNextButton = bool.Parse(EnableNextButtonElement.Value);
                EnableNextButtonElement.Remove();
            }

            XElement NextButtonNameElement = Document.Root.Element("NextButtonName");
            if (NextButtonNameElement != null)
            {
                NextButtonName = NextButtonNameElement.Value;
                NextButtonNameElement.Remove();
            }

            XElement SaveOnNextElement = Document.Root.Element("SaveOnNext");
            if (SaveOnNextElement != null)
            {
                SaveOnNext = bool.Parse(SaveOnNextElement.Value);
                SaveOnNextElement.Remove();
            }

            XElement NextButtonRedirectUrlElement = Document.Root.Element("NextButtonRedirectUrl");
            if (NextButtonRedirectUrlElement != null)
            {
                NextButtonRedirectUrl = NextButtonRedirectUrlElement.Value;
                NextButtonRedirectUrlElement.Remove();
            }

            XElement NextButtonRedirectUrlContextElement = Document.Root.Element("NextButtonRedirectUrlContext");
            if (NextButtonRedirectUrlContextElement != null)
            {
                NextButtonRedirectUrlContext = NextButtonRedirectUrlContextElement.Value;
                NextButtonRedirectUrlContextElement.Remove();
            }

            string NextButtonOnClientClick = String.Empty;
            XElement NextButtonOnClientClickElement = Document.Root.Element("NextButtonOnClientClick");
            if (NextButtonOnClientClickElement != null)
            {
                NextButtonOnClientClick = NextButtonOnClientClickElement.Value;

                NextButtonOnClientClickElement.Remove();
            }

            if (EnableNextButton)
            {
                XElement nextSaveCommand = new XElement("InsertUpdateSaveCommand",
                    new XElement("Name", "NextSaveCommand"),
                    new XElement("EntityID", EntityID),
                    new XElement("EntityInputType", EntityInputType),
                    new XElement("InsertCommand", InsertCommand),
                    new XElement("UpdateCommand", UpdateCommand),
                    new XElement("AfterInsertConfirmationMessage", AfterInsertConfirmationMessage),
                    new XElement("AfterUpdateConfirmationMessage", AfterUpdateConfirmationMessage),
                    new XElement("RedirectAfterInsert", RedirectAfterInsert),
                    new XElement("RedirectAfterUpdate", RedirectAfterUpdate),
                    new XElement("AfterUpdateRedirectUrl", AfterUpdateRedirectUrl),
                    new XElement("AfterInsertRedirectUrl", AfterInsertRedirectUrl),
                    new XElement("AfterUpdateRedirectUrlContext", AfterUpdateRedirectUrlContext),
                    new XElement("AfterInsertRedirectUrlContext", AfterInsertRedirectUrlContext)
                    );
                ;

                XElement nextNavigateCommand = new XElement("NavigateToUrlCommand",
                                new XElement("Name", "NavigateOnNext"),
                                new XElement("EntityID", EntityID),
                                new XElement("NavigateUrl", NextButtonRedirectUrlElement.Value),
                                new XElement("Context", NextButtonRedirectUrlContext)
                                );


                XElement nextCommands = new XElement("Commands");

                if (SaveOnNext)
                {
                    nextCommands.Add(nextSaveCommand);
                }

                if (!String.IsNullOrEmpty(NextButtonRedirectUrl))
                {
                    nextCommands.Add(nextNavigateCommand);
                }

                nextButton = new XElement("Button",
                    new XElement("Name", "Next"),
                    new XElement("CssClass", "btn btn-primary"),
                    new XElement("Text", NextButtonName),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "true"),
                    new XElement("OnClientClick", NextButtonOnClientClick),
                    new XElement("OnClick", nextCommands)
                    );

            }


            //cancel
            XElement cancelButton = null;
            bool EnableCancelButton = true;
            XElement EnableCancelButtonElement = Document.Root.Element("EnableCancelButton");
            if (EnableCancelButtonElement != null)
            {
                EnableCancelButton = bool.Parse(EnableCancelButtonElement.Value);
                EnableCancelButtonElement.Remove();
            }

            if (EnableCancelButton)
            {
                cancelButton = new XElement("Button",
                    new XElement("Name", "Cancel"),
                    new XElement("Text", "Cancel"),
                    new XElement("CssClass", "btn btn-default"),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "false"),
                    new XElement("OnClick",
                        new XElement("Commands",
                            new XElement("NavigateToUrlCommand",
                                new XElement("Name", "NavigateOnCancel"),
                                new XElement("EntityID", EntityID),
                                new XElement("NavigateUrl", AfterCancelRedirectUrl),
                                new XElement("Context", AfterCancelRedirectUrlContext)
                                )
                            )
                        )
                    );

            }
            //buttons

            XElement buttonSection = new XElement("ButtonListSection",
                new XElement("Name", "Buttons"),
                new XElement("Type", "ButtonList"),
                new XElement("CssClass", "text-center"),
                new XElement("ContainerMode", "Plain"),
                new XElement("CssClass", "text-center"),
                new XElement("Mode", "All"),
                new XElement("Visible", true),
                new XElement("ContentPane", "Bottom"),
                new XElement("Permission",
                    new XElement("CheckPermission", false)
                ),
                new XElement("Buttons",
                    new XElement("CheckPermission", false)
                )
            );

            if (saveButton != null)
            {
                buttonSection.Element("Buttons").Add(saveButton);
            }

            if (nextButton != null)
            {
                buttonSection.Element("Buttons").Add(nextButton);
            }

            if (cancelButton != null)
            {
                buttonSection.Element("Buttons").Add(cancelButton);
            }


            if (Document.Root.Element("Sections").Element("ButtonListSection") == null)
            {
                Document.Root.Element("Sections").Add(buttonSection);
            }

            //add OnPageLoad action for edit screen
            bool UseDefaultCommand = false;
            string DefaultCommand = String.Empty;
            string RetrieveCommand = String.Empty;

            XElement UseDefaultCommandElement = Document.Root.Element("UseDefaultCommand");
            if (UseDefaultCommandElement != null)
            {
                UseDefaultCommand = bool.Parse(UseDefaultCommandElement.Value);
                UseDefaultCommandElement.Remove();
            }

            XElement DefaultCommandElement = Document.Root.Element("DefaultCommand");
            if (DefaultCommandElement != null)
            {
                DefaultCommand = DefaultCommandElement.Value;
                DefaultCommandElement.Remove();
            }

            XElement RetrieveCommandElement = Document.Root.Element("RetrieveCommand");
            if (RetrieveCommandElement != null)
            {
                RetrieveCommand = RetrieveCommandElement.Value;
                RetrieveCommandElement.Remove();
            }


            XElement editOnPageLoad = new XElement("OnPageLoad",
                        new XElement("Commands",
                            new XElement("DefaultOrPopulateScreenCommand",
                                new XElement("Name", "DefaultOrPopulateScreen"),
                                new XElement("EntityID", EntityID),
                                new XElement("EntityInputType", EntityInputType),
                                new XElement("DefaultCommand", DefaultCommand),
                                new XElement("RetrieveCommand", RetrieveCommand)
                                )
                            )
                        );

            if (Document.Root.Element("OnPageLoad") == null)
            {
                Document.Root.Add(editOnPageLoad);
            }

            AddInsertEditRenderPageSectionsCommand(Document, EntityID, EntityInputType);
        }

        private static void ProcessSearchScreen(XDocument Document)
        {


            //add OnPageLoad action for edit screen
            bool UseDefaultCommand = false;
            string DefaultCommand = String.Empty;
            string RetrieveCommand = String.Empty;

            XElement UseDefaultCommandElement = Document.Root.Element("UseDefaultCommand");
            if (UseDefaultCommandElement != null)
            {
                UseDefaultCommand = bool.Parse(UseDefaultCommandElement.Value);
                UseDefaultCommandElement.Remove();
            }

            XElement DefaultCommandElement = Document.Root.Element("DefaultCommand");
            if (DefaultCommandElement != null)
            {
                DefaultCommand = DefaultCommandElement.Value;
                DefaultCommandElement.Remove();
            }

            XElement RetrieveCommandElement = Document.Root.Element("RetrieveCommand");
            if (RetrieveCommandElement != null)
            {
                RetrieveCommand = RetrieveCommandElement.Value;
                RetrieveCommandElement.Remove();
            }


            XElement OnPageLoad = new XElement("OnPageLoad",
                        new XElement("Commands",
                            new XElement("DefaultOrPopulateScreenCommand",
                                new XElement("Name", "DefaultOrPopulateScreen"),
                                new XElement("DefaultCommand", DefaultCommand)
                                )
                            )
                        );

            if (Document.Root.Element("OnPageLoad") == null)
            {
                Document.Root.Add(OnPageLoad);
            }
        }

        private static void ProcessListScreen(XDocument Document)
        {


            //add OnPageLoad action for edit screen
            bool UseDefaultCommand = false;
            string DefaultCommand = String.Empty;
            string RetrieveCommand = String.Empty;

            XElement UseDefaultCommandElement = Document.Root.Element("UseDefaultCommand");
            if (UseDefaultCommandElement != null)
            {
                UseDefaultCommand = bool.Parse(UseDefaultCommandElement.Value);
                UseDefaultCommandElement.Remove();
            }

            XElement DefaultCommandElement = Document.Root.Element("DefaultCommand");
            if (DefaultCommandElement != null)
            {
                DefaultCommand = DefaultCommandElement.Value;
                DefaultCommandElement.Remove();
            }

            XElement RetrieveCommandElement = Document.Root.Element("RetrieveCommand");
            if (RetrieveCommandElement != null)
            {
                RetrieveCommand = RetrieveCommandElement.Value;
                RetrieveCommandElement.Remove();
            }


            XElement OnPageLoad = new XElement("OnPageLoad",
                        new XElement("Commands",
                            new XElement("DefaultOrPopulateScreenCommand",
                                new XElement("Name", "DefaultOrPopulateScreen"),
                                new XElement("DefaultCommand", DefaultCommand)
                                )
                            )
                        );

            if (Document.Root.Element("OnPageLoad") == null)
            {
                Document.Root.Add(OnPageLoad);
            }
        }

        private static void ProcessListEditScreen(XDocument Document)
        {



        }

        private static void ProcessCommentsScreen(XDocument Document)
        {
            string EntityID = String.Empty;
            string EntityInputType = String.Empty;
            string EntityType = String.Empty;

            XElement EntityIDElement = Document.Root.Element("EntityID");
            if (EntityIDElement != null)
            {
                EntityID = EntityIDElement.Value;
                EntityIDElement.Remove();

            }


            XElement EntityInputTypeElement = Document.Root.Element("EntityInputType");
            if (EntityInputTypeElement != null)
            {
                EntityInputType = EntityInputTypeElement.Value;
                EntityInputTypeElement.Remove();

            }

            XElement EntityTypeElement = Document.Root.Element("EntityType");
            if (EntityTypeElement != null)
            {
                EntityType = EntityTypeElement.Value;
                EntityTypeElement.Remove();

            }


            string AfterCancelConfirmationMessage = String.Empty;
            XElement AfterCancelConfirmationMessageElement = Document.Root.Element("AfterCancelConfirmationMessage");
            if (AfterCancelConfirmationMessageElement != null)
            {
                AfterCancelConfirmationMessage = AfterCancelConfirmationMessageElement.Value;
                AfterCancelConfirmationMessageElement.Remove();

            }


            string RedirectAfterCancel = String.Empty;
            XElement RedirectAfterCancelElement = Document.Root.Element("RedirectAfterCancel");
            if (RedirectAfterCancelElement != null)
            {
                RedirectAfterCancel = RedirectAfterCancelElement.Value;
                RedirectAfterCancelElement.Remove();

            }

            string AfterCancelRedirectUrl = String.Empty;
            XElement AfterCancelRedirectUrlElement = Document.Root.Element("AfterCancelRedirectUrl");
            if (AfterCancelRedirectUrlElement != null)
            {
                AfterCancelRedirectUrl = AfterCancelRedirectUrlElement.Value;
                AfterCancelRedirectUrlElement.Remove();

            }

            string AfterCancelRedirectUrlContext = String.Empty;
            XElement AfterCancelRedirectUrlContextElement = Document.Root.Element("AfterCancelRedirectUrlContext");
            if (AfterCancelRedirectUrlContextElement != null)
            {
                AfterCancelRedirectUrlContext = AfterCancelRedirectUrlContextElement.Value;
                AfterCancelRedirectUrlContextElement.Remove();

            }

            //add edit section
            XElement EditSection = new XElement("EditSection",
                    new XElement("Name", "Enter your comments here"),
                    new XElement("Type", "Edit"),
                    new XElement("Mode", "All"),
                    new XElement("Visible", true),
                    new XElement("ContentPane", "Left"),
                    new XElement("Permission",
                        new XElement("CheckPermission", false)
                    ),
                    new XElement("Controls",
                        new XElement("TextAreaControl",
                            new XElement("Name", "Comments"),
                            new XElement("LabelRenderingMode", "None"),
                            new XElement("IsRequired", "true"),
                            new XElement("Rows", "6"),
                            new XElement("Width", "100%")
                            )
                    )
                );

            Document.Root.Element("Sections").Add(EditSection);


            //create button section
            XElement saveButton = new XElement("Button",
                    new XElement("Name", "Save"),
                    new XElement("Text", "Save"),
                    new XElement("CssClass", "btn btn-primary"),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "true"),
                    new XElement("OnClick",
                        new XElement("Commands",
                            new XElement("InsertUpdateSaveCommand",
                                new XElement("Name", "SaveCommand"),
                                new XElement("EntityID", EntityID),
                                new XElement("EntityInputType", EntityInputType),
                                new XElement("InsertCommand", "Comment_SaveComment"),
                                new XElement("UpdateCommand", "Comment_SaveComment"),
                                new XElement("AfterInsertConfirmationMessage", "Comment added"),
                                new XElement("AfterUpdateConfirmationMessage", "Comment added"),
                                new XElement("RedirectAfterInsert", "false"),
                                new XElement("RedirectAfterUpdate", "false")
                                ),
                            new XElement("SetControlPropertyCommand",
                                new XElement("Name", "ClearComments"),
                                new XElement("ControlName", "Comments"),
                                new XElement("PropertyName", "Value"),
                                new XElement("PropertyValue", "")
                                )
                            )
                        )

                );


            XElement cancelButton = new XElement("Button",
                    new XElement("Name", "Cancel"),
                    new XElement("Text", "Cancel"),
                    new XElement("CssClass", "btn btn-default"),
                    new XElement("Visible", "true"),
                    new XElement("CausesValidation", "false"),
                    new XElement("OnClick",
                        new XElement("Commands",
                            new XElement("NavigateToUrlCommand",
                                new XElement("Name", "NavigateOnCancel"),
                                new XElement("EntityID", EntityID),
                                new XElement("NavigateUrl", AfterCancelRedirectUrl),
                                new XElement("Context", AfterCancelRedirectUrlContext)
                                )
                            )
                        )
                    );



            XElement buttonSection = new XElement("ButtonListSection",
                new XElement("Name", "Buttons"),
                new XElement("Type", "ButtonList"),
                new XElement("CssClass", "text-center"),
                new XElement("ContainerMode", "Plain"),
                new XElement("Mode", "All"),
                new XElement("Visible", true),
                new XElement("ContentPane", "Bottom"),
                new XElement("Permission",
                    new XElement("CheckPermission", false)
                ),
                new XElement("Buttons",
                    new XElement("CheckPermission", false),
                    saveButton,
                    cancelButton
                )
            );

            Document.Root.Element("Sections").Add(buttonSection);

            //create grid section
            XElement gridSection = new XElement("GridSection",
                new XElement("Name", "Existing Comments"),
                new XElement("Type", "Grid"),
                new XElement("Mode", "All"),
                new XElement("Visible", true),
                new XElement("ContentPane", "Left"),
                new XElement("Permission",
                    new XElement("CheckPermission", false)
                ),

                new XElement("Grid",
                    new XElement("Name", "Existing Comments"),
                    new XElement("SelectDataCommand", "Comment_GetCommentsByEntityID"),
                    new XElement("AllowPaging", "true"),
                    new XElement("PageSize", "20"),
                    new XElement("AllowSorting", "true"),
                    new XElement("SortOrder", "Descending"),
                    new XElement("Columns",
                        new XElement("BoundGridColumn",
                            new XElement("HeaderText", "Comments"),
                            new XElement("DataField", "Comments"),
                            new XElement("SortExpression", "Comments"),
                            new XElement("ColumnType", "BoundGridColumn"),
                            new XElement("IncludeInExport", "true")
                        ),
                        new XElement("BoundGridColumn",
                            new XElement("HeaderText", "Date/Time"),
                            new XElement("DataField", "CreatedOn"),
                            new XElement("SortExpression", "CreatedOn"),
                            new XElement("DataFormatString", "{0:MMM dd yyyy HH:MM:ss}"),
                            new XElement("ColumnType", "BoundGridColumn"),
                            new XElement("IncludeInExport", "true")
                        ),
                        new XElement("BoundGridColumn",
                            new XElement("HeaderText", "Created By"),
                            new XElement("DataField", "FullName"),
                            new XElement("SortExpression", "FullName"),
                            new XElement("ColumnType", "BoundGridColumn"),
                            new XElement("IncludeInExport", "true")
                        )
                    )
                )
           );

            Document.Root.Element("Sections").Add(gridSection);

            if (Document.Root.Element("DataCommands") == null)
            {
                Document.Root.Add(new XElement("DataCommands"));
            }

            XElement comment_GetCommentsByEntityID_dataCommand = new XElement("DataCommand",
                  new XElement("Name", "Comment_GetCommentsByEntityID"),
                  new XElement("Parameters",
                        new XElement("Parameter",
                                new XElement("Name", "@EntityID"),
                                new XElement("InputType", EntityInputType),
                                new XElement("InputKey", EntityID)
                            ),
                        new XElement("Parameter",
                                new XElement("Name", "@EntityType"),
                                new XElement("InputType", "Constant"),
                                new XElement("InputKey", EntityType)
                            )
                      )
                );

            XElement comment_SaveComment_dataCommand = new XElement("DataCommand",
                  new XElement("Name", "Comment_SaveComment"),
                  new XElement("Parameters",
                        new XElement("Parameter",
                                new XElement("Name", "@EntityID"),
                                new XElement("InputType", EntityInputType),
                                new XElement("InputKey", EntityID)
                            ),
                        new XElement("Parameter",
                                new XElement("Name", "@EntityType"),
                                new XElement("InputType", "Constant"),
                                new XElement("InputKey", EntityType)
                            ),
                        new XElement("Parameter",
                                new XElement("Name", "@Comments"),
                                new XElement("InputType", "Control"),
                                new XElement("InputKey", "Comments")
                            ),
                        new XElement("Parameter",
                                new XElement("Name", "@CreatedBy"),
                                new XElement("InputType", "Special"),
                                new XElement("InputKey", "UserName")
                            )

                      )
                );

            Document.Root.Element("DataCommands").Add(comment_GetCommentsByEntityID_dataCommand);
            Document.Root.Element("DataCommands").Add(comment_SaveComment_dataCommand);

            AddInsertEditRenderPageSectionsCommand(Document, EntityID, EntityInputType);

        }

        private static void ProcessCriteriaListEditScreen(XDocument Document)
        {

            bool UseDefaultCommand = false;
            string DefaultCommand = String.Empty;
            string RetrieveCommand = String.Empty;

            XElement UseDefaultCommandElement = Document.Root.Element("UseDefaultCommand");
            if (UseDefaultCommandElement != null)
            {
                UseDefaultCommand = bool.Parse(UseDefaultCommandElement.Value);
                UseDefaultCommandElement.Remove();
            }

            XElement DefaultCommandElement = Document.Root.Element("DefaultCommand");
            if (DefaultCommandElement != null)
            {
                DefaultCommand = DefaultCommandElement.Value;
                DefaultCommandElement.Remove();
            }



            XElement OnPageLoad = new XElement("OnPageLoad",
                        new XElement("Commands",
                            new XElement("DefaultOrPopulateScreenCommand",
                                new XElement("Name", "DefaultOrPopulateScreen"),
                                new XElement("DefaultCommand", DefaultCommand)
                                )
                            )
                        );

            if (Document.Root.Element("OnPageLoad") == null)
            {
                Document.Root.Add(OnPageLoad);
            }

        }

        private static void ProcessPickerScreen(XDocument Document)
        {


            XElement UseTitleCommandInInsertMode = Document.Root.Element("UseTitleCommandInInsertMode");
            if (UseTitleCommandInInsertMode != null)
            {
                UseTitleCommandInInsertMode.Remove();
            }

            XElement AddPermission = Document.Root.Element("AddPermission");
            if (AddPermission != null)
            {
                AddPermission.Remove();
            }


            XElement SaveAction = Document.Root.Element("SaveAction");
            if (SaveAction != null)
            {
                SaveAction.Remove();
            }

        }




        
    }
}
