using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    public class Applicant
    {
        [JsonProperty("crystalId")]
        public string crystalId { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
        [JsonProperty("userId")]
        public int userId { get; set; }
    }


    public class DemandChannel
    {
        [JsonProperty("code")]
        public string code { get; set; }
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("label")]
        public string label { get; set; }
        [JsonProperty("usage")]
        public int usage { get; set; }
        [JsonProperty("whenCreated")]
        public DateTime whenCreated { get; set; }
        [JsonProperty("whenUpdated")]
        public object whenUpdated { get; set; }
        [JsonProperty("whoCreatedId")]
        public object whoCreatedId { get; set; }
        [JsonProperty("whoUpdatedId")]
        public object whoUpdatedId { get; set; }
    }

    public class Message
    {
        [JsonProperty("createdByMe")]
        public bool createdByMe { get; set; }
        [JsonProperty("createdByProfessional")]
        public bool createdByProfessional { get; set; }
        [JsonProperty("demandId")]
        public int demandId { get; set; }
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }
        public string MessagesDisplay { get {
                message = message.Replace("&lt;", "<");
                message = message.Replace("&gt;", ">");
                return message;
            } }

        [JsonProperty("validated")]
        public bool validated { get; set; }
        [JsonProperty("whenCreated")]
        public DateTime whenCreated { get; set; }
        [JsonProperty("whoCreatedId")]
        public int whoCreatedId { get; set; }
        [JsonProperty("whoCreatedUserDisplayName")]
        public string whoCreatedUserDisplayName { get; set; }
    }

    public class Owner
    {
        [JsonProperty("crystalId")]
        public string crystalId { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
        [JsonProperty("userId")]
        public int userId { get; set; }
    }

    public class ResponseChannel
    {
        [JsonProperty("code")]
        public string code { get; set; }
        [JsonProperty("id")]
        public int id { get; set; }
         [JsonProperty("label")]
        public string label { get; set; }
         [JsonProperty("usage")]
        public int usage { get; set; }
         [JsonProperty("whenCreated")]
        public DateTime whenCreated { get; set; }
         [JsonProperty("whenUpdated")]
        public object whenUpdated { get; set; }
         [JsonProperty("whoCreatedId")]
        public object whoCreatedId { get; set; }
         [JsonProperty("whoUpdatedId")]
        public object whoUpdatedId { get; set; }
    }

    public class UserDemands
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("whenCreated")]
        public DateTime whenCreated { get; set; }
        [JsonProperty("whenUpdated")]
        public DateTime? whenUpdated { get; set; }
        [JsonProperty("whoCreatedId")]
        public int whoCreatedId { get; set; }
        [JsonProperty("whoUpdatedId")]
        public int? whoUpdatedId { get; set; }
        [JsonProperty("applicationDescription")]
        public string applicationDescription { get; set; }
        [JsonProperty("applicationId")]
        public int? applicationId { get; set; }
        [JsonProperty("applicationName")]
        public string applicationName { get; set; }
        [JsonProperty("scopeDescription")]
        public string scopeDescription { get; set; }
        [JsonProperty("scopeId")]
        public object scopeId { get; set; }
        [JsonProperty("scopeName")]
        public string scopeName { get; set; }
         [JsonProperty("applicant")]
        public Applicant applicant { get; set; }
         [JsonProperty("applicantId")]
        public int applicantId { get; set; }
         [JsonProperty("attachments")]
        public object attachments { get; set; }
         [JsonProperty("barcode")]
        public string barcode { get; set; }
         [JsonProperty("canDelete")]
        public bool canDelete { get; set; }
         [JsonProperty("canEdit")]
        public bool canEdit { get; set; }
         [JsonProperty("canRead")]
        public bool canRead { get; set; }
         [JsonProperty("civility")]
        public string civility { get; set; }
         [JsonProperty("demandChannel")]
        public DemandChannel demandChannel { get; set; }
         [JsonProperty("demandChannelId")]
        public int demandChannelId { get; set; }
         [JsonProperty("email")]
        public string email { get; set; }
         [JsonProperty("firstname")]
        public string firstname { get; set; }
         [JsonProperty("formData")]
        public object formData { get; set; }
        [JsonProperty("keywords")]
        public string keywords { get; set; }
         [JsonProperty("lockResult")]
        public object lockResult { get; set; }
         [JsonProperty("messages")]
        public List<Message> messages { get; set; }
         [JsonProperty("note")]
        public string note { get; set; }
         [JsonProperty("owner")]
        public Owner owner { get; set; }
         [JsonProperty("ownerId")]
        public int? ownerId { get; set; }
         [JsonProperty("phone")]
        public string phone { get; set; }
         [JsonProperty("read")]
        public bool read { get; set; }
         [JsonProperty("reason")]
        public string reason { get; set; }
        [JsonProperty("responseChannel")]
        public ResponseChannel responseChannel { get; set; }
         [JsonProperty("responseChannelId")]
        public int responseChannelId { get; set; }
         [JsonProperty("status")]
        public int status { get; set; }
         [JsonProperty("surname")]
        public string surname { get; set; }
         [JsonProperty("whenClosed")]
        public DateTime? whenClosed { get; set; }
         [JsonProperty("DocumentId")]
        public string DocumentId { get; set; }
         [JsonProperty("applicantUserDisplayName")]
        public string applicantUserDisplayName { get; set; }
         [JsonProperty("applicantUserId")]
        public int applicantUserId { get; set; }
         [JsonProperty("demandChannelName")]
        public string demandChannelName { get; set; }
         [JsonProperty("duration")]
        public int? duration { get; set; }
         [JsonProperty("lastMessage")]
        public string lastMessage { get; set; }
         [JsonProperty("messagesText")]
        public string messagesText { get; set; }
         [JsonProperty("ownerUserDisplayName")]
        public string ownerUserDisplayName { get; set; }
         [JsonProperty("ownerUserId")]
        public int? ownerUserId { get; set; }
         [JsonProperty("ownerUserUid")]
        public string ownerUserUid { get; set; }
         [JsonProperty("reasonLabel")]
        public string reasonLabel { get; set; }
         [JsonProperty("responseChannelName")]
        public string responseChannelName { get; set; }
         [JsonProperty("sourceMessage")]
        public string sourceMessage { get; set; }
         [JsonProperty("statusLabel")]
        public string statusLabel { get; set; }
         [JsonProperty("whoCreatedUserDisplayName")]
        public string whoCreatedUserDisplayName { get; set; }
         [JsonProperty("whoCreatedUserId")]
        public int whoCreatedUserId { get; set; }
         [JsonProperty("whoUpdatedUserDisplayName")]
        public string whoUpdatedUserDisplayName { get; set; }
         [JsonProperty("whoUpdatedUserId")]
        public int? whoUpdatedUserId { get; set; }
        public string StateIcon
        {
            get { return Expanded ? "expanded_icon.png" : "collapsed_icon.png"; }
        }
        private bool _expanded;
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                }
            }
        }
    }

    public enum UserDemandStatus
    {
        /// <summary>
        /// A traiter.
        /// </summary>
        [EnumMember]
        Pending = 0,
        /// <summary>
        /// En cours.
        /// </summary>
        [EnumMember]
        Working = 1,
        /// <summary>
        /// Traitée.
        /// </summary>
        [EnumMember]
        Closed = 2,
        /// <summary>
        /// Archivée.
        /// </summary>
        [EnumMember]
        Archived = 3
    }

    public class RequestAddMessageToDemands
    {
        public List<object> errors { get; set; }
        public string message { get; set; }
        public bool success { get; set; }

    }
}
