﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ServerApp
{
    

    public class ServerData
    {

        string responsesC0; 
        string responsesC1; 

        public void start()
        {

            responsesC0 = "";
            responsesC1 = "";

        }

        public void receiveData(int client, string p_s) 
        {          

            //armazena o último dado recebido
            if (client == 0)
                responsesC1 = p_s;
            else
                responsesC0 = p_s;

        }
        public string sendData(int client) //separar leitura arquivo obj
        {

            string tempString = "";
            if (client == 0)
            {
                tempString = responsesC0;
                responsesC0 = "";
            }
            else
            {
                tempString = responsesC1;
                responsesC1 = "";
            }

            return tempString;

        }
    }
}
