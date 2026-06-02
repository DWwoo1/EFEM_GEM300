using System;
using System.Collections.Generic;

using EFEM.Defines.Job;
using EFEM.Jobs.Binding;
using EFEM.Jobs.Domain;
using EFEM.Jobs.Manager;

namespace EFEM.Jobs.Presentation
{
    /// <summary>
    /// JobManager와 Binder의 읽기 전용 정보를 UI 트리 구조로 변환한다.
    ///
    /// 책임:
    /// - ControlJob / ProcessJob / BindingSnapshot을 읽어 JobTreeNode 생성
    ///
    /// 하지 않는 일:
    /// - Job 상태 변경
    /// - Substrate 바인딩 수행
    /// - WinForms 컨트롤 직접 접근
    /// </summary>
    public sealed class JobTreeBuilder
    {
        private readonly IJobManager _jobManager;
        private readonly ISubstrateJobBinder _binder;

        public JobTreeBuilder(
            IJobManager jobManager,
            ISubstrateJobBinder binder)
        {
            if (jobManager == null)
                throw new ArgumentNullException(nameof(jobManager));

            _jobManager = jobManager;
            _binder = binder;
        }

        public JobTreeNode Build()
        {
            var root = new JobTreeNode
            {
                Text = "Jobs",
                NodeType = "Root"
            };

            root.Children.Add(BuildControlJobsNode());
            root.Children.Add(BuildUnlinkedProcessJobsNode());

            return root;
        }

        private JobTreeNode BuildControlJobsNode()
        {
            var node = new JobTreeNode
            {
                Text = "Control Jobs",
                NodeType = "ControlJobGroup"
            };

            IReadOnlyList<ControlJob> controlJobs =
                _jobManager.GetAllControlJobs();

            if (controlJobs == null)
                return node;

            foreach (ControlJob controlJob in controlJobs)
            {
                if (controlJob == null)
                    continue;

                node.Children.Add(BuildControlJobNode(controlJob));
            }

            return node;
        }

        private JobTreeNode BuildControlJobNode(ControlJob controlJob)
        {
            var node = new JobTreeNode
            {
                Text = controlJob.Id + " [" + controlJob.State + "]",
                Detail = "StartMode=" + controlJob.StartMode
                    + ", ProcessOrderManagement=" + controlJob.ProcessOrderManagement,
                NodeType = "ControlJob",
                SourceId = controlJob.Id
            };

            AddArrayNode(node, "CarrierInputIds", controlJob.CarrierInputIds);
            AddArrayNode(node, "CurrentProcessJobIds", controlJob.CurrentProcessJobIds);

            var processJobsNode = new JobTreeNode
            {
                Text = "Process Jobs",
                NodeType = "ProcessJobGroup"
            };

            /*
             * GetLinkedProcessJobs()를 사용하면 JobManager의 정렬 정책을 따른다.
             */
            IReadOnlyList<ProcessJob> processJobs =
                _jobManager.GetLinkedProcessJobs(controlJob.Id);

            if (processJobs != null)
            {
                foreach (ProcessJob processJob in processJobs)
                {
                    if (processJob == null)
                        continue;

                    processJobsNode.Children.Add(
                        BuildProcessJobNode(
                            controlJob.Id,
                            processJob));
                }
            }

            node.Children.Add(processJobsNode);

            return node;
        }

        private JobTreeNode BuildProcessJobNode(
            string controlJobId,
            ProcessJob processJob)
        {
            JobBindingSnapshot bindingSnapshot = null;

            if (_binder != null)
            {
                bindingSnapshot =
                    _binder.GetBindingSnapshot(
                        controlJobId,
                        processJob.Id);
            }

            string bindingText = bindingSnapshot == null
                ? "Unknown"
                : bindingSnapshot.Status.ToString();

            var node = new JobTreeNode
            {
                Text = processJob.Id
                    + " ["
                    + processJob.State
                    + "] [Binding: "
                    + bindingText
                    + "]",
                Detail = "Recipe="
                    + processJob.RecipeId
                    + ", MaterialOrder="
                    + processJob.MaterialOrder
                    + ", StartMode="
                    + processJob.StartMode,
                NodeType = "ProcessJob",
                SourceId = processJob.Id
            };

            node.Children.Add(CreateAttributeNode("MaterialFormat", processJob.MaterialFormat.ToString()));
            node.Children.Add(CreateAttributeNode("RecipeMethod", processJob.RecipeMethod.ToString()));
            node.Children.Add(CreateAttributeNode("RecipeId", processJob.RecipeId));

            node.Children.Add(BuildMaterialInfoNode(processJob));
            node.Children.Add(BuildBindingNode(bindingSnapshot));
            node.Children.Add(BuildRecipeParameterNode(processJob));

            AddArrayNode(node, "PauseEventIds", processJob.PauseEventIds);

            return node;
        }

        private JobTreeNode BuildMaterialInfoNode(ProcessJob processJob)
        {
            var node = new JobTreeNode
            {
                Text = "MaterialInfo",
                NodeType = "MaterialInfo"
            };

            if (processJob.MaterialInfo == null || processJob.MaterialInfo.Count == 0)
            {
                node.Children.Add(new JobTreeNode
                {
                    Text = "No Carrier / No Slot",
                    Detail = "No substrate binding target",
                    NodeType = "MaterialInfoEmpty"
                });

                return node;
            }

            foreach (KeyValuePair<string, IReadOnlyList<int>> item in processJob.MaterialInfo)
            {
                string carrierId = item.Key;
                IReadOnlyList<int> slots = item.Value;

                bool hasCarrier = !string.IsNullOrWhiteSpace(carrierId);
                bool hasSlots = slots != null && slots.Count > 0;

                if (!hasCarrier && !hasSlots)
                {
                    node.Children.Add(new JobTreeNode
                    {
                        Text = "No Carrier / No Slot",
                        Detail = "No substrate binding target",
                        NodeType = "MaterialInfoEmpty"
                    });

                    continue;
                }

                if (hasCarrier && !hasSlots)
                {
                    node.Children.Add(new JobTreeNode
                    {
                        Text = carrierId,
                        Detail = "No Slot",
                        NodeType = "Carrier",
                        SourceId = carrierId
                    });

                    continue;
                }

                var carrierNode = new JobTreeNode
                {
                    Text = carrierId,
                    NodeType = "Carrier",
                    SourceId = carrierId
                };

                foreach (int slot in slots)
                {
                    carrierNode.Children.Add(new JobTreeNode
                    {
                        Text = "Slot " + slot,
                        NodeType = "Slot",
                        SourceId = carrierId + ":" + slot
                    });
                }

                node.Children.Add(carrierNode);
            }

            return node;
        }

        private JobTreeNode BuildBindingNode(JobBindingSnapshot snapshot)
        {
            var node = new JobTreeNode
            {
                Text = "Binding",
                NodeType = "Binding"
            };

            if (snapshot == null)
            {
                node.Children.Add(new JobTreeNode
                {
                    Text = "Unknown",
                    NodeType = "BindingStatus"
                });

                return node;
            }

            node.Children.Add(new JobTreeNode
            {
                Text = "Status: " + snapshot.Status,
                Detail = snapshot.Message,
                NodeType = "BindingStatus"
            });

            if (snapshot.Materials == null || snapshot.Materials.Count == 0)
                return node;

            foreach (JobBindingSnapshot.Material material in snapshot.Materials)
            {
                var materialNode = new JobTreeNode
                {
                    Text = material.CarrierId
                        + " / Slot "
                        + material.Slot
                        + " ["
                        + material.Status
                        + "]",
                    Detail = material.Message,
                    NodeType = "MaterialBinding",
                    SourceId = material.CarrierId + ":" + material.Slot
                };

                materialNode.Children.Add(CreateAttributeNode("PortId", material.PortId.ToString()));
                materialNode.Children.Add(CreateAttributeNode("SubstrateKey", material.SubstrateId));
                materialNode.Children.Add(CreateAttributeNode("BoundControlJobId", material.BoundControlJobId));
                materialNode.Children.Add(CreateAttributeNode("BoundProcessJobId", material.BoundProcessJobId));
                materialNode.Children.Add(CreateAttributeNode("BoundRecipeId", material.BoundRecipeId));

                node.Children.Add(materialNode);
            }

            return node;
        }

        private JobTreeNode BuildRecipeParameterNode(ProcessJob processJob)
        {
            var node = new JobTreeNode
            {
                Text = "Recipe Parameters",
                NodeType = "RecipeParameterGroup"
            };

            string[] names = processJob.RecipeParameterNames ?? new string[0];
            string[] values = processJob.RecipeParameterValues ?? new string[0];

            int count = Math.Max(names.Length, values.Length);

            for (int i = 0; i < count; ++i)
            {
                string name = i < names.Length ? names[i] : string.Empty;
                string value = i < values.Length ? values[i] : string.Empty;

                node.Children.Add(new JobTreeNode
                {
                    Text = name + " = " + value,
                    NodeType = "RecipeParameter"
                });
            }

            return node;
        }

        private JobTreeNode BuildUnlinkedProcessJobsNode()
        {
            var node = new JobTreeNode
            {
                Text = "Unlinked Process Jobs",
                NodeType = "UnlinkedProcessJobGroup"
            };

            IReadOnlyList<ProcessJob> processJobs =
                _jobManager.GetAllProcessJobs();

            if (processJobs == null)
                return node;

            foreach (ProcessJob processJob in processJobs)
            {
                if (processJob == null)
                    continue;

                string controlJobId =
                    _jobManager.GetControlJobIdOrDefault(processJob.Id);

                if (!string.IsNullOrWhiteSpace(controlJobId))
                    continue;

                node.Children.Add(
                    BuildProcessJobNode(
                        string.Empty,
                        processJob));
            }

            return node;
        }

        private static JobTreeNode CreateAttributeNode(
            string name,
            string value)
        {
            return new JobTreeNode
            {
                Text = name + ": " + (value ?? string.Empty),
                NodeType = "Attribute"
            };
        }

        private static void AddArrayNode(
            JobTreeNode parent,
            string title,
            string[] values)
        {
            var node = new JobTreeNode
            {
                Text = title,
                NodeType = "Array"
            };

            if (values != null)
            {
                foreach (string value in values)
                {
                    node.Children.Add(new JobTreeNode
                    {
                        Text = value ?? string.Empty,
                        NodeType = "ArrayItem"
                    });
                }
            }

            parent.Children.Add(node);
        }

        private static void AddArrayNode(
            JobTreeNode parent,
            string title,
            uint[] values)
        {
            var node = new JobTreeNode
            {
                Text = title,
                NodeType = "Array"
            };

            if (values != null)
            {
                foreach (uint value in values)
                {
                    node.Children.Add(new JobTreeNode
                    {
                        Text = value.ToString(),
                        NodeType = "ArrayItem"
                    });
                }
            }

            parent.Children.Add(node);
        }
    }
}