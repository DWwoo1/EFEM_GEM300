using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EFEM.Defines.Common;
using EFEM.Defines.Job;
using EFEM.Jobs.Repository;

namespace EFEM.Jobs.Domain
{
    public sealed class ProcessJob : IEntity<string>
    {
        public string Id { get; private set; }

        public ProcessJobState State { get; private set; }

        public MaterialFormat MaterialFormat { get; private set; }

        public ProcessStartMode StartMode { get; private set; }

        public MaterialOrderMode MaterialOrder { get; private set; }

        public IReadOnlyDictionary<string, IReadOnlyList<int>> MaterialInfo { get; private set; }

        public RecipeMethod RecipeMethod { get; private set; }

        public string RecipeId { get; private set; }

        public string[] RecipeParameterNames { get; private set; }

        public string[] RecipeParameterValues { get; private set; }

        // PauseEvent Attribute.
        // ProcessJob Pause 시 사용할 Event ID 목록.
        public uint[] PauseEventIds { get; private set; }

        public ProcessJob(
            string id,
            ProcessJobState state,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues)
            : this(
                id,
                state,
                materialFormat,
                startMode,
                materialOrder,
                materialInfo,
                recipeMethod,
                recipeId,
                recipeParameterNames,
                recipeParameterValues,
                new uint[0])
        {
        }

        public ProcessJob(
            string id,
            ProcessJobState state,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues,
            uint[] pauseEventIds)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ProcessJobId is invalid.", nameof(id));

            Id = id;
            State = state;
            MaterialFormat = materialFormat;
            StartMode = startMode;
            MaterialOrder = materialOrder;
            MaterialInfo = materialInfo;
            RecipeMethod = recipeMethod;
            RecipeId = recipeId;
            RecipeParameterNames = recipeParameterNames ?? new string[0];
            RecipeParameterValues = recipeParameterValues ?? new string[0];
            PauseEventIds = pauseEventIds ?? new uint[0];
        }
        public override string ToString()
        {
            string materialInfoText = MaterialInfo == null
                ? string.Empty
                : string.Join(
                    ",",
                    MaterialInfo.Select(x =>
                    {
                        string values = x.Value == null
                            ? string.Empty
                            : string.Join(",", x.Value);

                        return string.IsNullOrEmpty(values)
                            ? "[" + x.Key + "]"
                            : "[" + x.Key + "," + values + "]";
                    }));

            return string.Format(
                "ProcessJobId={0}, State={1}, MaterialFormat={2}, StartMode={3}, MaterialOrder={4}, RecipeMethod={5}, RecipeId={6}, MaterialInfo=[{7}], RecipeParameterNames=[{8}], RecipeParameterValues=[{9}], PauseEventIds=[{10}]",
                Id,
                State,
                MaterialFormat,
                StartMode,
                MaterialOrder,
                RecipeMethod,
                RecipeId,
                materialInfoText,
                RecipeParameterNames == null ? string.Empty : string.Join(",", RecipeParameterNames),
                RecipeParameterValues == null ? string.Empty : string.Join(",", RecipeParameterValues),
                PauseEventIds == null ? string.Empty : string.Join(",", PauseEventIds));
        }
        public void ChangeAttributeInfo(
            ProcessJobState state,
            MaterialFormat materialFormat,
            ProcessStartMode startMode,
            MaterialOrderMode materialOrder,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo,
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues,
            uint[] pauseEventIds)
        {
            State = state;
            MaterialFormat = materialFormat;
            StartMode = startMode;
            MaterialOrder = materialOrder;
            MaterialInfo = materialInfo;
            RecipeMethod = recipeMethod;
            RecipeId = recipeId;
            RecipeParameterNames = recipeParameterNames ?? new string[0];
            RecipeParameterValues = recipeParameterValues ?? new string[0];
            PauseEventIds = pauseEventIds ?? new uint[0];
        }
        public void ChangeState(ProcessJobState state)
        {
            State = state;
        }

        public void ChangeRecipe(string recipeId)
        {
            RecipeId = recipeId;
        }

        public void ChangeStartMode(ProcessStartMode startMode)
        {
            StartMode = startMode;
        }

        public void ChangeMaterialOrder(MaterialOrderMode materialOrder)
        {
            MaterialOrder = materialOrder;
        }

        public void ChangePauseEventIds(uint[] pauseEventIds)
        {
            PauseEventIds = pauseEventIds ?? new uint[0];
        }

        public void ChangeRecipeParameters(
            string[] recipeParameterNames,
            string[] recipeParameterValues)
        {
            RecipeParameterNames = recipeParameterNames ?? new string[0];
            RecipeParameterValues = recipeParameterValues ?? new string[0];
        }

        public void ChangeMaterial(
            MaterialFormat materialFormat,
            IReadOnlyDictionary<string, IReadOnlyList<int>> materialInfo)
        {
            MaterialFormat = materialFormat;
            MaterialInfo = materialInfo;
        }

        public void ChangeRecipeInfo(
            RecipeMethod recipeMethod,
            string recipeId,
            string[] recipeParameterNames,
            string[] recipeParameterValues)
        {
            RecipeMethod = recipeMethod;
            RecipeId = recipeId;
            RecipeParameterNames = recipeParameterNames ?? new string[0];
            RecipeParameterValues = recipeParameterValues ?? new string[0];
        }
    }
}
