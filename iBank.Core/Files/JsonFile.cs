using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using PCLExt.FileStorage;
using PCLExt.FileStorage.Extensions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace iBank.Core.Files
{
    public abstract class JsonFile : BaseFile
    {
        /// <summary>
        /// Ignores readonly properties from BaseFile in the Json file.
        /// </summary>
        protected class BaseFileResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty>
                CreateProperties(Type type, MemberSerialization memberSerialization) =>
                base.CreateProperties(type, memberSerialization)
                    .Where(p => p.PropertyName != nameof(Name) &&
                                p.PropertyName != nameof(Path) &&
                                p.PropertyName != nameof(Exists) &&
                                p.PropertyName != nameof(Size) &&
                                p.PropertyName != nameof(CreationTime) &&
                                p.PropertyName != nameof(CreationTimeUTC) &&
                                p.PropertyName != nameof(LastAccessTime) &&
                                p.PropertyName != nameof(LastAccessTimeUTC) &&
                                p.PropertyName != nameof(LastWriteTime) &&
                                p.PropertyName != nameof(LastWriteTimeUTC))
                    .ToList();
        }

        protected virtual JsonSerializerSettings GetSettings() => new JsonSerializerSettings()
        {
            ContractResolver = new BaseFileResolver(),
            Formatting = Formatting.Indented
        };

        [DebuggerStepThrough]
        protected JsonFile(IFile file) : base(file)
        {
            Reload();
        }

        /// <summary>
        /// Will trigger Save() if field has changed.
        /// </summary>
        protected void SetValueIfChangedAndSave<TRet>(ref TRet backingField, TRet newValue)
        {
            if (!EqualityComparer<TRet>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;
                Save();
            }
        }

        public bool Reload()
        {
            try
            {
                var content = this.ReadAllText();
                if (string.IsNullOrEmpty(content))
                    this.WriteAllText(content = JsonConvert.SerializeObject(this, GetSettings()));

                JsonConvert.PopulateObject(content, this, GetSettings());
                return true;
            }
            catch (JsonSerializationException) // Json file is invalid, replace with the default valid one.
            {
                // Comment this to prevent replacing invalid json with default values.
                this.WriteAllText(JsonConvert.SerializeObject(this, GetSettings()));
                return false;
            }
        }
        public bool Save()
        {
            try
            {
                this.WriteAllText(JsonConvert.SerializeObject(this, GetSettings()));
                return true;
            }
            catch (JsonSerializationException)
            {
                return false;
            }
        }

        protected async Task<bool> ReloadAsync()
        {
            try
            {
                var content = await this.ReadAllTextAsync();
                if (string.IsNullOrEmpty(content))
                    await this.WriteAllTextAsync(content = JsonConvert.SerializeObject(this, GetSettings()));

                JsonConvert.PopulateObject(content, this, GetSettings());
                return true;
            }
            catch (JsonSerializationException) // Json file is invalid, replace with the default valid one.
            {
                // Comment this to prevent replacing with default.
                await this.WriteAllTextAsync(JsonConvert.SerializeObject(this, GetSettings()));
                return false;
            }
        }
        public async Task<bool> SaveAsync()
        {
            try
            {
                await this.WriteAllTextAsync(JsonConvert.SerializeObject(this, GetSettings()));
                return true;
            }
            catch (JsonSerializationException)
            {
                return false;
            }
        }
    }
}