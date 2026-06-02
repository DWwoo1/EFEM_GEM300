using EFEM.Jobs.Domain;

namespace EFEM.Jobs.Repository
{
    public sealed class ControlJobRecoveryJsonAdapter :
        IJobJsonEntityAdapter<ControlJob>
    {
        public ControlJob Load(string path)
        {
            var dto = JsonJobStorageFile.LoadOrBackup<ControlJobRecoveryDto>(path);

            if (dto == null)
                return null;

            return dto.ToDomain();
        }

        public void Save(string path, ControlJob entity)
        {
            var dto = ControlJobRecoveryDto.FromDomain(entity);

            JsonJobStorageFile.SaveAtomic(path, dto);
        }
    }
}