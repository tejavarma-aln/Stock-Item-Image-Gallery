using System.Collections.Generic;
using System.Text;

namespace ImageGallery
{
    static class ObjectHolder
    {

        static public List<ItemModel> _items;
        static public string URL;

        static public string getItemXML()
        {
            StringBuilder builder = new StringBuilder("<ENVELOPE><HEADER><VERSION>1</VERSION><TALLYREQUEST>EXPORT</TALLYREQUEST>");
            builder.Append("<TYPE>Collection</TYPE><ID>StockItemImages</ID></HEADER><BODY>");
            builder.Append("<DESC><STATICVARIABLES><SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT></STATICVARIABLES>");
            builder.Append("<TDL><TDLMESSAGE><COLLECTION Name='StockItemImages'>");
            builder.Append("<TYPE>StockItem</TYPE><COMPUTE>Description:$Description</COMPUTE><COMPUTE>BaseUnits:$BaseUnits</COMPUTE><COMPUTE>PartNo:$PartNo</COMPUTE>");
            builder.Append("</COLLECTION></TDLMESSAGE></TDL></DESC></BODY></ENVELOPE>");

            return builder.ToString();
        }

        static public string makeHostName(string host,string port)
        {
            URL = string.Format("http://{0}:{1}", host, port);
            return URL;
        }


    }
}
