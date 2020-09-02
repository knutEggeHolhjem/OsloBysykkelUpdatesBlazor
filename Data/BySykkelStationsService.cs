using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OsloBysykkelUpdatesBlazor.Data
{
    public class BySykkelStationsService
    {
        static readonly HttpClient client = new HttpClient();

        public BySykkelStationsService()
        {
            //Add header specified by the api. 
            client.DefaultRequestHeaders.Add("Client-Identifier", "KnutEggeHolhjem-OsloBysykkelUpdates");
        }

        /// <summary>
        /// Get station updates from oslobysykkel api.
        /// </summary>
        /// <returns>return a sorted list of updated stations</returns>
        public async Task<List<Station>> GetStationsAsync()
        {
            List<Station> stations = new List<Station>();
            try
            {
                //Make a async get request, response should be a jason with status of stations. 
                string response = await client.GetStringAsync("https://gbfs.urbansharing.com/oslobysykkel.no/station_status.json");

                //Deserialize response message to class genereated from jason. 
                StationStatusJson StationStatus = JsonConvert.DeserializeObject<StationStatusJson>(response);

                stations = StationStatus.data.stations;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            //sort the stations by the station id.
            stations = stations.OrderBy(station => station.station_id).ToList();
            return stations;
        }
    }
}
