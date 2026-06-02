using System;
using System.Windows.Forms;
using System.Collections.Generic;

using EFEM.Defines.Job;
using EFEM.Jobs.Binding;
using EFEM.Jobs.Manager;
using EFEM.Jobs.Presentation;

namespace FrameOfSystem3.Views.Operation
{
    /// <summary>
    /// 기존 WinForms 화면에 붙여서 사용하는 Job 모니터링 UserControl.
    ///
    /// 책임:
    /// - JobTreeBuilder가 만든 JobTreeNode를 TreeView에 표시한다.
    /// - 선택된 노드의 간단한 상세 정보를 표시한다.
    ///
    /// 하지 않는 일:
    /// - Job 상태 변경
    /// - Job 생성/삭제
    /// - Substrate 바인딩 판정
    /// </summary>
    public partial class Operation_JobInfo : UserControlForMainView.CustomView
    {
        private JobTreeBuilder _jobTreeBuilder;

        public Operation_JobInfo()
        {
            InitializeComponent();

            /*
             * 주의:
             * 여기서 JobManager.Instance를 직접 접근하지 않는다.
             *
             * 이유:
             * - UserControl은 WinForms Designer에서도 생성될 수 있다.
             * - JobManager가 아직 Configure되지 않았으면 Instance 접근 시 예외가 날 수 있다.
             *
             * 실제 서비스 객체는 InitializeServices()에서 주입한다.
             */
        }

        protected override void ProcessWhenActivation()
        {
            base.ProcessWhenActivation();

            InitializeServices(JobManager.Instance, SubstrateJobBindingService.Instance);
        }

        /// <summary>
        /// 외부 Form에서 JobManager와 Binder를 주입한다.
        /// JobManager.ConfigureDeferred 이후 호출해야 한다.
        /// </summary>
        public void InitializeServices(
            IJobManager jobManager,
            ISubstrateJobBinder binder)
        {
            if (jobManager == null)
                throw new ArgumentNullException(nameof(jobManager));

            _jobTreeBuilder = new JobTreeBuilder(
                jobManager,
                binder);

            RefreshJobTree();
        }

        /// <summary>
        /// 현재 Job 정보를 다시 읽어 TreeView를 갱신한다.
        /// </summary>
        public void RefreshJobTree()
        {
            if (_jobTreeBuilder == null)
                return;

            JobTreeNode root = _jobTreeBuilder.Build();

            _treeViewJobs.BeginUpdate();

            try
            {
                _treeViewJobs.Nodes.Clear();

                TreeNode rootTreeNode = ToTreeNode(root);

                _treeViewJobs.Nodes.Add(rootTreeNode);

                /*
                 * ExpandAll()을 사용하지 않는다.
                 * 노드 타입별 정책에 따라 필요한 부분만 펼친다.
                 */
                ApplyRefreshExpandPolicy(rootTreeNode);
            }
            finally
            {
                _treeViewJobs.EndUpdate();
            }
        }
        private static void ApplyRefreshExpandPolicy(TreeNode treeNode)
        {
            if (treeNode == null)
                return;

            JobTreeNode jobTreeNode = treeNode.Tag as JobTreeNode;

            if (ShouldExpandOnRefresh(jobTreeNode))
                treeNode.Expand();
            else
                treeNode.Collapse();

            foreach (TreeNode child in treeNode.Nodes)
                ApplyRefreshExpandPolicy(child);
        }
        private static bool ShouldExpandOnRefresh(JobTreeNode node)
        {
            if (node == null)
                return false;

            /*
             * Refresh 시 자동으로 펼칠 노드만 true.
             * 나머지 노드는 사용자가 직접 펼치도록 둔다.
             */
            switch (node.NodeType)
            {
                case "Root":
                case "ControlJobGroup":
                case "UnlinkedProcessJobGroup":
                case "ControlJob":
                case "ProcessJobGroup":
                    return true;

                /*
                 * ProcessJob부터는 상세 정보가 많으므로 접어둔다.
                 */
                case "ProcessJob":
                case "MaterialInfo":
                case "Binding":
                case "RecipeParameterGroup":
                case "Carrier":
                case "Slot":
                case "MaterialBinding":
                    return false;

                default:
                    return false;
            }
        }
        private void BtnRefresh_Click(object sender, System.EventArgs e)
        {
            RefreshJobTree();
        }

        private void TreeViewJobs_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            JobTreeNode node = e.Node == null
                ? null
                : e.Node.Tag as JobTreeNode;

            if (node == null)
            {
                txtJobDetail.Clear();
                return;
            }

            txtJobDetail.Text =
                "Type: " + node.NodeType + Environment.NewLine
                + "Id: " + node.SourceId + Environment.NewLine
                + "Text: " + node.Text + Environment.NewLine
                + "Detail: " + node.Detail;
        }
        private static TreeNode ToTreeNode(JobTreeNode source)
        {
            if (source == null)
                return new TreeNode(string.Empty);

            var node = new TreeNode(source.Text);
            node.Tag = source;

            foreach (JobTreeNode child in source.Children)
                node.Nodes.Add(ToTreeNode(child));

            return node;
        }
        private void GetAllChildNodes(TreeNode parentNode, List<TreeNode> result)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                result.Add(childNode);

                GetAllChildNodes(childNode, result);
            }
        }
        private void BtnExecuteJobCommand(object sender, EventArgs e)
        {
            if (sender.Equals(btnAbortAllJobs))
            {
                TreeNode selectedNode = _treeViewJobs.SelectedNode;

                if (selectedNode == null)
                    return;

                List<TreeNode> childNodes = new List<TreeNode>();

                GetAllChildNodes(selectedNode, childNodes);

                List<string> jobIds = new List<string>();
                foreach (TreeNode node in childNodes)
                {
                    JobTreeNode jobTreeNode = node.Tag as JobTreeNode;
                    if (jobTreeNode == null)
                        continue;

                    if (jobTreeNode.NodeType == "ProcessJob")
                    {
                        jobIds.Add(jobTreeNode.SourceId);
                    }
                    Console.WriteLine($"{jobTreeNode.NodeType} : {jobTreeNode.SourceId}");
                }

                foreach (var item in jobIds)
                {
                    JobManager.Instance.RequestProcessJobCommand(item, ProcessJobCommand.Abort);
                }

                RefreshJobTree();
            }
        }
    }
}
