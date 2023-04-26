
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SapWcf.PP0060;

[ServiceContract]
public interface ISapPP0060

{
    //http://infheaidrdb01.kolon.com:51000/dir/wsdl?p=ic/42a49c5c2d203ba2af9ecab97078e47d
    [OperationContract]
    [XmlSerializerFormat()]
  
    PP0060_Response SI_GRP_PP0060_SO(PP0060_Request request);

}

