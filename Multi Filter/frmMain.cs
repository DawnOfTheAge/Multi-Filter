using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Multi_Filter
{
    public partial class frmMain : Form
    {
        #region Enums

        public enum MultiFilterHierarchy
        {
            Unknown,
            Root,
            Search,
            Match
        }

        public enum MultiFilterCriteria
        {
            Unknown,
            And,
            Or
        }

        public enum MatchCriteria
        {
            Unknown,
            And,
            Or
        } 

        #endregion

        #region Data Types
        
        [Serializable]    
        public class TagClass
        {
            public TagClass()
            {
                CaseSensitive = false;
                Hierarchy = MultiFilterHierarchy.Unknown;
                Criteria = MultiFilterCriteria.Unknown;
                Enabled = true;
                Text = "";
            }

            public TagClass(bool caseSensitive, MultiFilterHierarchy hierarchy, MultiFilterCriteria criteria, bool enabled, string text)
            {
                CaseSensitive = caseSensitive;
                Hierarchy = hierarchy;
                Criteria = criteria;
                Enabled = enabled;
                Text = text;
            }

            public bool CaseSensitive
            {
                get;
                set;
            }

            public MultiFilterHierarchy Hierarchy
            {
                get;
                set;
            }

            public MultiFilterCriteria Criteria
            {
                get;
                set;
            }

            public bool Enabled
            {
                get;
                set;
            }

            public string Text
            {
                get;
                set;
            }
        }

        [Serializable]
        public class SearchProperties
        {
            public SearchProperties(string name, MatchCriteria matchCriteria)
            {
                Match = new List<MatchProperties>();

                Name = name;
                Criteria = matchCriteria;
            }

            public SearchProperties()
            {
                Match = new List<MatchProperties>();
            }

            public List<MatchProperties> Match
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public MatchCriteria Criteria
            {
                get;
                set;
            }

            public void ZapMatches()
            {
                Match = new List<MatchProperties>();
            }
        }

        [Serializable]
        public class MatchProperties
        {
            public MatchProperties()
            { 
            }

            public MatchProperties(string text, bool caseSensitive, bool enabled)
            {
                MatchText = text;
                CaseSensitive = caseSensitive;
                Enabled = enabled;
            }

            public string MatchText
            {
                get;
                set;
            }

            public bool CaseSensitive
            {
                get;
                set;
            }

            public bool Enabled
            {
                get;
                set;
            }
        }

        #endregion

        #region Constants
        
        // Xml tag for node, e.g. 'node' in case of <node></node>
        private const string XmlNodeTag = "node";

        // Xml attributes for node e.g. <node text="Asia" tag="" 
        // imageindex="1"></node>
        private const string XmlNodeTextAtt = "text";
        private const string XmlNodeTagAtt = "tag";
        private const string XmlNodeImageIndexAtt = "imageindex"; 

        #endregion

        #region Data Members
        
        private string[] loadedLine;

        private ContextMenu m_ContextMenu;

        private XmlReadWrite m_XmlReadWrite;

        private List<SearchProperties> m_Search;

        #endregion

        public frmMain()
        {
            InitializeComponent();
        }

        #region Initializes
        
        private void InitializeTree()
        {
            try
            {
                string fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Multi Filter.xml";

                tvSearchs.Nodes.Clear();

                TreeNode tn = tvSearchs.Nodes.Add("Searches");
                tn.Tag = "root";

                if (File.Exists(fileName))
                {
                    mnuLoadSearchs(null, null);

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Initialize Tree Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeGrid()
        {
            try
            {
                gdvLines.AutoResizeColumns();
                gdvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                gdvLines.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                gdvLines.Columns.Add("colLine", "Line Number");
                gdvLines.Columns.Add("colText", "Text");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Initialize Grid Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 

        #endregion

        #region GUI

        private void mnuFile_Click(object sender, EventArgs e)
        {
            try
            {
                string filename;
                string[] gridLine;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Log Files|*.*";
                openFileDialog.Title = "Select a Log File";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filename = openFileDialog.FileName;
                }
                else
                {
                    return;
                }

                loadedLine = File.ReadAllLines(filename);

                if (loadedLine != null)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    for (int i = 0; i < loadedLine.Length; i++)
                    {
                        gridLine = new string[2];

                        gridLine[0] = (i + 1).ToString();
                        gridLine[1] = loadedLine[i];

                        int row = gdvLines.Rows.Add(gridLine);
                    }

                    Cursor.Current = Cursors.Default;
                    lblNumberOfLines.Text = loadedLine.Length + " Lines";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            try
            {
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exit Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                m_XmlReadWrite = new XmlReadWrite();

                m_Search = new List<SearchProperties>();

                InitializeGrid();
                InitializeTree();

                frmMain_Resize(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Main Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            try
            {
                tvSearchs.Location = new Point(10, 50);
                tvSearchs.Size = new Size(Width / 5, ((Height / 8) * 6));

                gdvLines.Location = new Point((tvSearchs.Left + tvSearchs.Width + 20), tvSearchs.Top);
                gdvLines.Size = new Size(((Width / 10) * 7), ((Height / 8) * 6));

                lblNumberOfLines.Location = new Point(gdvLines.Left, gdvLines.Top - 20);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Resize Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tvSearchs_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                MenuItem[] menuItems;

                if (e.Button == MouseButtons.Right)
                {
                    TreeNode tn = tvSearchs.SelectedNode;

                    if (tn != null)
                    {
                        Type type = tn.Tag.GetType();

                        if (type == typeof(string))
                        {
                            menuItems = new MenuItem[3];

                            menuItems[0] = new MenuItem("Add Search", mnuAddSearch);
                            menuItems[1] = new MenuItem("Load Searchs", mnuLoadSearchs);
                            menuItems[2] = new MenuItem("Save Searchs", mnuSaveSearchs);

                            m_ContextMenu = new ContextMenu(menuItems);
                            tvSearchs.ContextMenu = m_ContextMenu;
                        }

                        if (type == typeof(SearchProperties))
                        {
                            menuItems = new MenuItem[4];

                            menuItems[0] = new MenuItem("Perform Search", mnuPerformSearch);
                            menuItems[1] = new MenuItem("Add Match", mnuAddMatch);
                            menuItems[2] = new MenuItem("Remove This Search", mnuRemoveSearch);
                            menuItems[3] = new MenuItem("AND / OR Search", mnuAndOrSearch);

                            m_ContextMenu = new ContextMenu(menuItems);
                            tvSearchs.ContextMenu = m_ContextMenu;
                        }

                        if (type == typeof(MatchProperties))
                        {
                            menuItems = new MenuItem[3];

                            menuItems[0] = new MenuItem("Enable / Disable Match", mnuEnableDisableMatch);
                            menuItems[1] = new MenuItem("True / False Case Sevsitive Match", mnuTrueFalseCaseSensitiveMatch);
                            menuItems[2] = new MenuItem("Remove This Match", mnuRemoveMatch);

                            m_ContextMenu = new ContextMenu(menuItems);
                            tvSearchs.ContextMenu = m_ContextMenu;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Menus Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        

        #endregion

        #region Load & Save XML File

        private void mnuSaveSearchs(object sender, EventArgs e)
        {
            try
            {
                string fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Multi Filter.xml";

                ReadTree();

                if ((m_Search != null) && (m_Search.Count > 0))
                {
                    File.Delete(fileName);
                    m_XmlReadWrite.WriteToXmlFile(fileName, m_Search);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Searchs Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuLoadSearchs(object sender, EventArgs e)
        {
            try
            {
                string fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Multi Filter.xml";

                m_Search = m_XmlReadWrite.ReadFromXmlFile<List<SearchProperties>>(fileName);

                if (m_Search == null)
                {
                    m_Search = new List<SearchProperties>();
                }

                tvSearchs.Nodes[0].Nodes.Clear();

                CreateTree();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Searchs Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tree Stuff

        private void ReadTree()
        {
            try
            {
                if (tvSearchs == null)
                {
                    return;
                }

                if (tvSearchs.Nodes == null)
                {
                    return;
                }

                if (tvSearchs.Nodes[0].Nodes == null)
                {
                    return;
                }

                m_Search = new List<SearchProperties>();

                for (int i = 0; i < tvSearchs.Nodes[0].Nodes.Count; i++)
                {
                    if (tvSearchs.Nodes[0].Nodes[i].Tag != null)
                    {
                        SearchProperties currentSearchProperties = (SearchProperties)(tvSearchs.Nodes[0].Nodes[i].Tag);
                        currentSearchProperties.ZapMatches();

                        if (tvSearchs.Nodes[0].Nodes[i].Nodes != null)
                        {
                            for (int j = 0; j < tvSearchs.Nodes[0].Nodes[i].Nodes.Count; j++)
                            {
                                if (tvSearchs.Nodes[0].Nodes[i].Nodes[j].Tag != null)
                                {
                                    MatchProperties currentMatchProperties = (MatchProperties)(tvSearchs.Nodes[0].Nodes[i].Nodes[j].Tag);

                                    currentSearchProperties.Match.Add(currentMatchProperties);
                                }
                            }
                        }

                        m_Search.Add(currentSearchProperties);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read Tree Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateTree()
        {
            try
            {
                if (m_Search != null)
                {
                    for (int i = 0; i < m_Search.Count; i++)
                    {
                        TreeNode tnSearch = tvSearchs.Nodes[0].Nodes.Add(m_Search[i].Name + " {" + m_Search[i].Criteria + "}");
                        tnSearch.Tag = m_Search[i];

                        if (m_Search[i].Match != null)
                        {
                            for (int j = 0; j < m_Search[i].Match.Count; j++)
                            {
                                TreeNode tnMatch = tvSearchs.Nodes[0].Nodes[i].Nodes.Add(m_Search[i].Match[j].MatchText + " {Case Sensitive " + m_Search[i].Match[j].CaseSensitive + "}");
                                tnMatch.Tag = m_Search[i].Match[j];
                                tnMatch.ForeColor = (m_Search[i].Match[j].Enabled) ? default(Color) : Color.Red;
                            }
                        }
                    }

                    tvSearchs.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Create Tree Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Search

        private void mnuAddSearch(object sender, EventArgs e)
        {
            try 
	        {	        
		        string searchName = Microsoft.VisualBasic.Interaction.InputBox("", "Add Search Name", "", 0, 0);

                if (!string.IsNullOrEmpty(searchName))
                {
                    TreeNode tn = tvSearchs.Nodes[0].Nodes.Add(searchName);
                    tn.Tag = new SearchProperties(searchName, MatchCriteria.And);

                    tn.Text += " - {AND}";
                }

                tvSearchs.ExpandAll();
	        }
	        catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuPerformSearch(object sender, EventArgs e)
        {
            try 
	        {	        
		        int numberOfLines = 0;
            
                TreeNode tn = tvSearchs.SelectedNode;

                SearchProperties searchProperties = (SearchProperties)(tn.Tag);

                string[] gridLine;

                if (tn != null)
                {
                    List<MatchProperties> lMatch = new List<MatchProperties>();

                    for (int i = 0; i < tn.Nodes.Count; i++)
                    {
                        MatchProperties currentMatchProperties = (MatchProperties)(tn.Nodes[i].Tag);

                        if (currentMatchProperties.Enabled)
                        {
                            lMatch.Add(currentMatchProperties);
                        }
                    }

                    gdvLines.Rows.Clear();

                    if (loadedLine == null)
                    {
                        return;
                    }

                    for (int i = 0; i < loadedLine.Length; i++)
                    {
                        int searchSuccess = (searchProperties.Criteria == MatchCriteria.And) ? 1 : 0;

                        for (int j = 0; j < lMatch.Count; j++)
                        {
                            bool searchFlag;

                            if (lMatch[j].CaseSensitive)
                            {
                                searchFlag = loadedLine[i].IndexOf(lMatch[j].MatchText) != -1;
                            }
                            else
                            {
                                searchFlag = loadedLine[i].ToLower().IndexOf(lMatch[j].MatchText.ToLower()) != -1;
                            }

                            if (searchFlag)
                            {
                                if (searchProperties.Criteria == MatchCriteria.And)
                                {
                                    searchSuccess *= 1;
                                }
                                else
                                {
                                    searchSuccess++;
                                }
                            }
                            else
                            {
                                if (searchProperties.Criteria == MatchCriteria.And)
                                {
                                    searchSuccess *= 0;
                                }
                            }
                        }

                        switch (searchProperties.Criteria)
                        {
                            case MatchCriteria.And:
                                if (searchSuccess == 1)
                                {
                                    gridLine = new string[2];

                                    gridLine[0] = (i + 1).ToString();
                                    gridLine[1] = loadedLine[i];

                                    gdvLines.Rows.Add(gridLine);

                                    numberOfLines++;
                                }
                                break;

                            case MatchCriteria.Or:
                                if (searchSuccess > 0)
                                {
                                    gridLine = new string[2];

                                    gridLine[0] = (i + 1).ToString();
                                    gridLine[1] = loadedLine[i];

                                    gdvLines.Rows.Add(gridLine);

                                    numberOfLines++;                            
                                }
                                break;

                            default:
                                break;
                        }
                    }

                    lblNumberOfLines.Text = "Lines " + numberOfLines.ToString();
                }
	        }
	        catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Perform Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuRemoveSearch(object sender, EventArgs e)
        {
            try 
	        {	        
		        TreeNode tn = tvSearchs.SelectedNode;

                if (tn == null)
                {
                    return;
                }

                string sText = tn.Text;

                for (int i = 0; i < tvSearchs.Nodes[0].Nodes.Count; i++)
                {
                    SearchProperties currentSearchProperties = (SearchProperties)(tvSearchs.Nodes[0].Nodes[i].Tag);

                    if (currentSearchProperties.Name == sText)
                    {
                        tvSearchs.Nodes[0].Nodes.RemoveAt(i);

                        return;
                    }
                }

                tvSearchs.ExpandAll();
	        }
	        catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Remove Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        

        private void mnuAndOrSearch(object sender, EventArgs e)
        {
            try
            {
                TreeNode tn = tvSearchs.SelectedNode;

                SearchProperties searchProperties = (SearchProperties)(tn.Tag);

                if (tn != null)
                {
                    if (searchProperties.Criteria == MatchCriteria.And)
                    {
                        searchProperties.Criteria = MatchCriteria.Or;
                    }
                    else
                    {
                        searchProperties.Criteria = MatchCriteria.And;
                    }

                    tn.Text = searchProperties.Name + " {" + searchProperties.Criteria + "}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AND/OR Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Match
        
        private void mnuAddMatch(object sender, EventArgs e)
        {
            try
            {
                TreeNode tn = tvSearchs.SelectedNode;

                if (tn == null)
                {
                    return;
                }

                string matchName = Microsoft.VisualBasic.Interaction.InputBox("", "Add Match Name", "", 0, 0);

                if (!string.IsNullOrEmpty(matchName))
                {
                    TreeNode newMatch = tn.Nodes.Add(matchName);
                    newMatch.Tag = new MatchProperties(matchName, false, true);
                }

                tvSearchs.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Match Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuRemoveMatch(object sender, EventArgs e)
        {
            try
            {
                TreeNode tn = tvSearchs.SelectedNode;
                TreeNode tnParent = tn.Parent;

                if (tn == null)
                {
                    return;
                }

                string sText = tn.Text;

                if (tnParent.Nodes != null)
                {
                    for (int i = 0; i < tnParent.Nodes.Count; i++)
                    {
                        MatchProperties currentMatchProperties = (MatchProperties)(tnParent.Nodes[i].Tag);

                        if (currentMatchProperties.MatchText == sText)
                        {
                            tnParent.Nodes.RemoveAt(i);

                            return;
                        }
                    }

                    tvSearchs.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Remove Match Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuEnableDisableMatch(object sender, EventArgs e)
        {
            try
            {
                TreeNode tn = tvSearchs.SelectedNode;

                MatchProperties matchProperties = (MatchProperties)(tn.Tag);

                if (tn != null)
                {
                    if (matchProperties.Enabled)
                    {
                        matchProperties.Enabled = false;
                        tn.ForeColor = Color.Red;
                    }
                    else
                    {
                        matchProperties.Enabled = true;
                        tn.ForeColor = default(Color);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Enable/Disable Match Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuTrueFalseCaseSensitiveMatch(object sender, EventArgs e)
        {
            try
            {
                TreeNode tn = tvSearchs.SelectedNode;

                MatchProperties matchProperties = (MatchProperties)(tn.Tag);

                if (tn != null)
                {
                    if (matchProperties.CaseSensitive)
                    {
                        matchProperties.CaseSensitive = false;
                    }
                    else
                    {
                        matchProperties.CaseSensitive = true;
                    }

                    tn.Text = matchProperties.MatchText + " {Case Sensitive " + matchProperties.CaseSensitive + "}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "True/False Case Sensitive Match Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion        
    }
}
