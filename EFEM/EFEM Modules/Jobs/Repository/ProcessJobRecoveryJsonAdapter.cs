using EFEM.Jobs.Domain;

namespace EFEM.Jobs.Repository
{
    public sealed class ProcessJobRecoveryJsonAdapter :
        IJobJsonEntityAdapter<ProcessJob>
    {
        public ProcessJob Load(string path)
        {
            var dto = JsonJobStorageFile.LoadOrBackup<ProcessJobRecoveryDto>(path);

            if (dto == null)
                return null;

            return dto.ToDomain();
        }

        public void Save(string path, ProcessJob entity)
        {
            var dto = ProcessJobRecoveryDto.FromDomain(entity);

            JsonJobStorageFile.SaveAtomic(path, dto);
        }
    }
}