using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vp_webservice_dotnet_sample.vp_webservice_auth;
using vp_webservice_dotnet_sample.vp_webservice_documents;
using vp_webservice_dotnet_sample.vp_webservice_documenttasks;

namespace vp_webservice_dotnet_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Auth Client about to start please wait...");
            AuthClient auth = new AuthClient();
            Console.WriteLine("Auth Client started trying to log in please wait...");
            String sessionId = auth.login("admin", "admin");
            Console.WriteLine("Logged in successfully with Session ID : " + sessionId);
            DocumentTasksClient tasksClient = new DocumentTasksClient();
            Console.WriteLine("Document Task Client started please wait...");
            DocumentQueueEntryInformation entryInformation = tasksClient.getNextDocumentInTaskQueue(sessionId, "INDEXING");

            if(entryInformation == null)
            {
                Console.WriteLine("No Document on the queue found");
            }
            else
            {
                Console.WriteLine("Document ID : " + entryInformation.documenId);
                Console.WriteLine("Queue ID : " + entryInformation.queueId);
                Console.WriteLine("Document Queue Entry ID : " + entryInformation.documentQueueEntryId);
                DocumentsClient documentsClient = new DocumentsClient();
                document document = documentsClient.getDocument(sessionId, entryInformation.documenId);
                Console.WriteLine("Document Content Type : " + document.contentType);
                Console.WriteLine("Document Archive Date : " + document.archiveDate);
                Console.WriteLine("Document Original Name : " + document.originalFileName);
                Console.WriteLine("Document Node name : " + document.nodeName);
                indexValue[] indexValues = document.indexValues;
                foreach(indexValue iv in indexValues)
                {
                    Console.WriteLine("Index name : " + iv.indexName + " with value : " + iv.indexValue1);
                }
                Console.WriteLine("Getting derived documents please wait...");
                document[] listDocuments = documentsClient.getDerivedDocuments(sessionId, entryInformation.documenId);
                foreach(document d in listDocuments)
                {
                    Console.WriteLine("Document Content Type : " + d.contentType);
                    Console.WriteLine("Document Archive Date : " + d.archiveDate);
                    Console.WriteLine("Document Original Name : " + d.originalFileName);
                    Console.WriteLine("Document Node name : " + d.nodeName);
                }



            }
            Console.ReadLine();
           
        }
    }
}
