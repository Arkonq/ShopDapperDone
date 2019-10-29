////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiContentElementCommissionByProviderClass
    {
        [DataMember]
        public string type;
        [DataMember]
        public string id;
        [DataMember]
        public string title;
        [DataMember]
        public string message;
        [DataMember(Name = "params")]
        public QiwiParamsContentElementClass[] _params;
        [DataMember]
        public string name;
        [DataMember]
        public string value;
        [DataMember]
        public QiwiSemanticsClass semantics;
        [DataMember]
        public QiwiValidatorClass validator;
        [DataMember]
        public QiwiViewClass view;
        [DataMember]
        public bool? sensitiveData = null;
        [DataMember]
        public bool? hideFromConfirmationScreen = null;
    }
}
