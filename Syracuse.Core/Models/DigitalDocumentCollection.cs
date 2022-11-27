using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    public class DigitalDocumentCollection
    {
        /// <summary>
        /// Indique la présence d'une table des matières
        /// </summary>
        [JsonProperty("hasContentTable")]

        public bool HasContentTable { get; set; }

        /// <summary>
        /// Nombre total de documents.
        /// </summary>
        [JsonProperty("totalCount")]
        public int Count { get; set; }

        /// <summary>
        /// Position actuelle dans le viewer
        /// </summary>
        [JsonProperty("currentIndex")]

        public int CurrentIndex { get; set; }

        /// <summary>
        /// Liste des documents.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonProperty("documents")]

        public Collection<DigitalDocument> Documents { get; set; }

        /// <summary>
        /// Page actuelle
        /// </summary>
        [JsonProperty("page")]

        public int Page { get; set; }

        /// <summary>
        /// Index de début
        /// </summary>
        [JsonProperty("start")]

        public int Start { get; set; }

        /// <summary>
        /// Nombre de document total avant filtrage
        /// </summary>
        [JsonProperty("countBeforeFiltering")]

        public int TotalCount { get; set; }

        /// <summary>
        /// Identifiant du document mère de cette collection
        /// </summary>
        [JsonProperty("documentId")]
        public string DocumentId { get; set; }

        /// <summary>
        /// Nombre de document en cours de publication
        /// </summary>
        [JsonProperty("publishingCount")]

        public int PublishingCount { get; set; }
    }

    public class CopyrightData
    {
        public string font { get; set; }
        public string text { get; set; }
    }

    public class DigitalDocument
    {
        public object additionnalField1 { get; set; }
        public object additionnalField2 { get; set; }
        public object additionnalField3 { get; set; }
        public object additionnalField4 { get; set; }
        public object additionnalField5 { get; set; }
        public bool canDownload { get; set; }
        public bool canDownloadLowRes { get; set; }
        public bool canSkipCopyright { get; set; }
        public bool canSkipWatermark { get; set; }
        public bool canView { get; set; }
        public object code { get; set; }
        public int collectionIndex { get; set; }
        public string contract { get; set; }
        public string contractToApply { get; set; }
        public CopyrightData copyrightData { get; set; }
        public bool copyrightEnabled { get; set; }
        public object detailedEntry { get; set; }
        public List<object> documentData { get; set; }
        public string documentId { get; set; }
        public object endTimeCode { get; set; }
        public string fileName { get; set; }
        public string fileSize { get; set; }
        public object fileStreamHd { get; set; }
        public object fileStreamSd { get; set; }
        public object fullText { get; set; }
        public bool hasDetailedEntry { get; set; }
        public object history { get; set; }
        public int index { get; set; }
        public bool isEntryPoint { get; set; }
        public bool isPublishing { get; set; }
        public bool isWatermarkingDisabled { get; set; }
        public object licence { get; set; }
        public object link { get; set; }
        public object linkType { get; set; }
        public List<object> metaDatas { get; set; }
        public List<object> metaDatasSynch { get; set; }
        public bool metadata { get; set; }
        public string mimeType { get; set; }
        public object mimeTypeFlow { get; set; }
        public string noticeType { get; set; }
        public int order { get; set; }
        public string parentContract { get; set; }
        public string parentDocumentId { get; set; }
        public string parentRule { get; set; }
        public object parentTitle { get; set; }
        public object privateNote { get; set; }
        public object publicNote { get; set; }
        public string solrFieldParentTitle { get; set; }
        public string solrFieldTitle { get; set; }
        public object startTimeCode { get; set; }
        public object subMimeType { get; set; }
        public object summary { get; set; }
        public string thumbnailPath { get; set; }
        public int tileHeight { get; set; }
        public int tileOverlap { get; set; }
        public int tileSize { get; set; }
        public int tileWidth { get; set; }
        public object timeCodeContract { get; set; }
        public string title { get; set; }
        public bool useThumbnail { get; set; }
        public string watermark { get; set; }
        public string whenUpdated { get; set; }
    }
}