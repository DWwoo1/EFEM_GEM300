using System.Collections.Generic;

using EFEM.Defines.Common;
using EFEM.Defines.Job;
using EFEM.Jobs.Domain;

namespace EFEM.Jobs.Repository
{
    public sealed class ProcessJobRecoveryDto
    {
        public string Id { get; set; }

        public ProcessJobState State { get; set; }

        public MaterialFormat MaterialFormat { get; set; }

        public ProcessStartMode StartMode { get; set; }

        public MaterialOrderMode MaterialOrder { get; set; }

        public Dictionary<string, List<int>> MaterialInfo { get; set; }

        public RecipeMethod RecipeMethod { get; set; }

        public string RecipeId { get; set; }

        public string[] RecipeParameterNames { get; set; }

        public string[] RecipeParameterValues { get; set; }

        public uint[] PauseEventIds { get; set; }

        public static ProcessJobRecoveryDto FromDomain(ProcessJob job)
        {
            if (job == null)
                return null;

            return new ProcessJobRecoveryDto
            {
                Id = job.Id,
                State = job.State,
                MaterialFormat = job.MaterialFormat,
                StartMode = job.StartMode,
                MaterialOrder = job.MaterialOrder,
                MaterialInfo = CopyMaterialInfo(job.MaterialInfo),
                RecipeMethod = job.RecipeMethod,
                RecipeId = job.RecipeId,
                RecipeParameterNames = job.RecipeParameterNames ?? new string[0],
                RecipeParameterValues = job.RecipeParameterValues ?? new string[0],
                PauseEventIds = job.PauseEventIds ?? new uint[0]
            };
        }

        public ProcessJob ToDomain()
        {
            return new ProcessJob(
                Id,
                State,
                MaterialFormat,
                StartMode,
                MaterialOrder,
                ToReadOnlyMaterialInfo(MaterialInfo),
                RecipeMethod,
                RecipeId,
                RecipeParameterNames ?? new string[0],
                RecipeParameterValues ?? new string[0],
                PauseEventIds ?? new uint[0]);
        }

        private static Dictionary<string, List<int>> CopyMaterialInfo(
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo)
        {
            var result = new Dictionary<string, List<int>>();

            if (materialInfo == null)
                return result;

            foreach (var item in materialInfo)
            {
                if (string.IsNullOrWhiteSpace(item.Key))
                    continue;

                var values = new List<int>();

                if (item.Value != null)
                {
                    foreach (var slot in item.Value)
                        values.Add(slot);
                }

                result[item.Key] = values;
            }

            return result;
        }

        private static IReadOnlyDictionary<string, IReadOnlyList<int>> ToReadOnlyMaterialInfo(
            Dictionary<string, List<int>> materialInfo)
        {
            var result = new Dictionary<string, IReadOnlyList<int>>();

            if (materialInfo == null)
                return result;

            foreach (var item in materialInfo)
            {
                if (string.IsNullOrWhiteSpace(item.Key))
                    continue;

                result[item.Key] = item.Value == null
                    ? new List<int>()
                    : new List<int>(item.Value);
            }

            return result;
        }
    }
}