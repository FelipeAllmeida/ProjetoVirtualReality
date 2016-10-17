using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace serverApp
{
  
    class MyTcpListener
    {

        public static void Main()
        {
            ServerApp.ServerData tempserver = new ServerApp.ServerData();
            TcpListener server = null;
            try
            {
                // Porta 13000
                Int32 port = 123;
                //IP local
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // cria o server
                server = new TcpListener(localAddr, port);

                // inicia o server
                server.Start();

                List<TcpClient> clients = new List<TcpClient>();
                // buffer de leitura
                Byte[] bytes = new Byte[1024];
                String data = null;
                int counter = 0;
                tempserver.start();

                // entra no loop de leitura
                while (true)
                {
                   
                    // Cria um bloqueio aguardando conexões      
                    if (clients.Count < 2)
                    {
                        if (clients.Count < 1)
                            Console.Write("Server pronto... Conectar Client 1 ");
                        else
                            Console.Write("Aguardando Client 2 ");

                        TcpClient client = server.AcceptTcpClient();
                        clients.Add(client);

                        counter++;
                    }
                    Console.Write("conectados");


                    while (clients.Count == 2)
                    {
                        data = null;
                      

                        for (int ci = 0; ci < clients.Count; ci++)
                        {

                           
                            // Loop para receber todos os dados enviados pelos clientes
                            try
                            {
                                // cria uma stream 
                                NetworkStream stream = clients[ci].GetStream();

                                int i;
                                bytes = new Byte[1024];
                                stream.ReadTimeout = 1;
                                if ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                    // transforma os dados em ASCII string
                                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                    Console.Write("data recebida: " + data +" por "+ci +"/n");
                                    tempserver.receiveData(ci, data);
                                    stream.Flush();

                                }
                            }
                            catch(IOException e)
                            {


                            }
                            string response = "";
                            response = tempserver.sendData(ci);
                          //  response = "tempo " + System.DateTime.Now.Millisecond;

                            if (response.Length > 0)
                            {
                                NetworkStream stream = clients[ci].GetStream();
                                Console.Write("data enviada " + response + " por " + ci + "/n");


                                byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);

                                // Send back a response.
                                stream.Write(msg, 0, msg.Length);
                                stream.Flush();
                            }

                        }
                    }
                    
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();

           
        }
        
    }
}