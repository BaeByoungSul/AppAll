using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.ServiceModel;
using Models.Database;
using MyClientLib;
using Services.DbService;
using Services.FileService;

namespace CommonLib
{
    public static class MyStatic
    {


        static readonly string dbAddr_http = ConfigurationManager.AppSettings["DB_SERVICE_HTTP"];
        static readonly string fileAddr_tcp = ConfigurationManager.AppSettings["FILE_SERVICE_TCP"];


        public static DbReturn GetDataSet(MyCommand cmd)
        {
            try
            {
                DataSet ds = new DataSet();

                BasicHttpBinding myBinding = GetDbHttpBinding();
                EndpointAddress myEndpoint = new EndpointAddress(dbAddr_http);

                ChannelFactory<IDbService> myChannelFactory = new ChannelFactory<IDbService>(myBinding, myEndpoint);

                // Create a channel.
                IDbService _cli = myChannelFactory.CreateChannel();

                var rtn = _cli.GetDataSet(cmd);
                Console.WriteLine(rtn.ReturnCD);

                //rtn.RtnBody.RemoveAttribute("xmlns"); // StringReader할 때 오류가 나서 추가함
                Console.WriteLine(rtn.RtnBody?.OuterXml);
                StringReader theReader = new StringReader(rtn.RtnBody.OuterXml);

                //DataSet ds = new DataSet();
                ds.ReadXml(theReader, XmlReadMode.ReadSchema);

                ((IClientChannel)_cli).Close();

                myChannelFactory.Close();
                return new DbReturn()
                {
                    ReturnCD = rtn.ReturnCD,
                    ReturnMsg = rtn.ReturnMsg,
                    ReturnDs = ds
                };
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public static DbReturn ExecNonQuery(List<MyCommand> cmds)
        {
            try
            {
                DataSet ds = new DataSet();
                BasicHttpBinding myBinding = GetDbHttpBinding();
                EndpointAddress myEndpoint = new EndpointAddress(dbAddr_http);

                
                ChannelFactory<IDbService> myChannelFactory = new ChannelFactory<IDbService>(myBinding, myEndpoint);

                // Create a channel.
                IDbService _cli = myChannelFactory.CreateChannel();

                var rtn = _cli.ExecNonQuery(cmds);
                Console.WriteLine(rtn.ReturnCD);

                //rtn.RtnBody.RemoveAttribute("xmlns"); // StringReader할 때 오류가 나서 추가함
                Console.WriteLine(rtn.RtnBody?.OuterXml);
                StringReader theReader = new StringReader(rtn.RtnBody.OuterXml);
                
                ds.ReadXml(theReader, XmlReadMode.ReadSchema);

                ((IClientChannel)_cli).Close();

                myChannelFactory.Close();
                return new DbReturn()
                {
                    ReturnCD = rtn.ReturnCD,
                    ReturnMsg = rtn.ReturnMsg,
                    ReturnDs = ds
                };
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static CheckFileResponse CheckFile(string fileName) {
            try
            {
                //BasicHttpBinding myBinding = GetFileHttpBinding();
                //EndpointAddress myEndpoint = new EndpointAddress(fileAddr_http);

                NetTcpBinding myBinding = GetFileTcpBinding();
                EndpointAddress myEndpoint = new EndpointAddress(fileAddr_tcp);


                ChannelFactory<IFileService> myChannelFactory = new ChannelFactory<IFileService>(myBinding, myEndpoint);

                // Create a channel.
                IFileService _cli = myChannelFactory.CreateChannel();

                var rtn = _cli.CheckFile(fileName);
                ((IClientChannel)_cli).Close();
                myChannelFactory.Close();

                return rtn;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public static DownloadResponse DownloadFile(string fileName)
        {
            try
            {
                //BasicHttpBinding myBinding = GetFileHttpBinding();
                //EndpointAddress myEndpoint = new EndpointAddress(fileAddr_http);

                NetTcpBinding myBinding = GetFileTcpBinding();
                EndpointAddress myEndpoint = new EndpointAddress(fileAddr_tcp);

                ChannelFactory<IFileService> myChannelFactory = new ChannelFactory<IFileService>(myBinding, myEndpoint);

                // Create a channel.
                IFileService _cli = myChannelFactory.CreateChannel();

                DownloadFile downloadFile = new DownloadFile()
                {
                    FileName = fileName
                };
                var rtn = _cli.DownloadFile(downloadFile);

                ((IClientChannel)_cli).Close();
                myChannelFactory.Close();

                return rtn;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
              
            }

           
        }
        public static UploadResponse UploadFile(UploadFile uploadFile)
        {
            try
            {
                //BasicHttpBinding myBinding = GetFileHttpBinding();
                //EndpointAddress myEndpoint = new EndpointAddress(fileAddr_http);


                NetTcpBinding myBinding = GetFileTcpBinding();
                EndpointAddress myEndpoint = new EndpointAddress(fileAddr_tcp);

                ChannelFactory<IFileService> myChannelFactory = new ChannelFactory<IFileService>(myBinding, myEndpoint);

                // Create a channel.
                IFileService _cli = myChannelFactory.CreateChannel();

                //CRRedist2008_x64.msi
                //string filepath = @"D:\Software\" + fileName;
                var rtn = _cli.UploadFile(uploadFile);
                //Console.WriteLine(rtn.ReturnCD);

                ((IClientChannel)_cli).Close();
                myChannelFactory.Close();
                return rtn;
            }
            catch (Exception)
            {

                throw;
            }

            

        }

        private static BasicHttpBinding GetDbHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding();

            binding.TransferMode = TransferMode.Streamed;
            //binding.MessageEncoding = WSMessageEncoding.Mtom;
            binding.Security.Mode = BasicHttpSecurityMode.None;

            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;

            binding.OpenTimeout = TimeSpan.FromMinutes(5);
            binding.CloseTimeout = TimeSpan.FromMinutes(5);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(15);
            binding.SendTimeout = TimeSpan.FromMinutes(15);

            binding.ReaderQuotas.MaxStringContentLength = 2147483647;

            return binding;
        }

        private static BasicHttpBinding GetFileHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding();

            binding.TransferMode = TransferMode.Streamed;
            binding.MessageEncoding = WSMessageEncoding.Mtom;
            binding.Security.Mode = BasicHttpSecurityMode.None;

            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferSize = 65536;

            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            binding.ReaderQuotas.MaxDepth = 2147483647;

            binding.OpenTimeout = TimeSpan.FromMinutes(5);
            binding.CloseTimeout = TimeSpan.FromMinutes(5);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(15);
            binding.SendTimeout = TimeSpan.FromMinutes(15);

            return binding;
        }
        public static NetTcpBinding GetFileTcpBinding()
        {
            var nettcpBinding = new NetTcpBinding();
            nettcpBinding.TransferMode = TransferMode.Streamed;

            nettcpBinding.Security.Mode = SecurityMode.None;
            nettcpBinding.MaxReceivedMessageSize = 2147483647;
            nettcpBinding.MaxBufferSize = 65536;

            nettcpBinding.OpenTimeout = TimeSpan.FromMinutes(5);
            nettcpBinding.CloseTimeout = TimeSpan.FromMinutes(5);
            nettcpBinding.ReceiveTimeout = TimeSpan.FromMinutes(15);
            nettcpBinding.SendTimeout = TimeSpan.FromMinutes(15);

            return nettcpBinding;
        }
    }
}
