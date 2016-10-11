using System;
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

        public void receiveData(int client, string p_s) //separar leitura arquivo obj
        {

            //delimitadores
            char[] delimiters = { '(',',',')' };
            string tempStr = "";

            string[] words = p_s.Split(delimiters);

            //aqui só inverte a ordem das palvras que recebeu
            for (int i = words.Length -1; i >= 0; i-- )
            {
                tempStr = tempStr + words[i] + ",";

            }

            //armazena o último dado recebido
            if (client == 0)
                responsesC1 = tempStr;
            else
                responsesC0 = tempStr;

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
