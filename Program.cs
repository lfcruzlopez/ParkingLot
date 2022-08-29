using System;
using System.Collections.Generic;
using System.Linq;

// To execute C#, please define "static void Main" on a class
// named Solution.


class ParkingLot{
    public string Location { get; set; }
    public List<Spot> Spots { get; set; }
    public List<Reservation> Reservations {get;set;}

    public ParkingLot(string location, List<Spot>  spots)
    {
        Location = location;
        Spots = spots ?? new List<Spot>();
        Reservations = new List<Reservation>();
    }

    public List<Spot> GetAvailableSpots(SpotSize size){

        return  Spots.Where(b=>b.IsAvailable == true && b.Size == size ).ToList();
    }

    public bool CheckIn(Reservation reservation){

            try
            {
                foreach(var spotId in reservation.Spots)
                {
                    ChangeSpotStatus(spotId, false);
                }
            }  
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
    }

    public bool CheckOut(Reservation reservation){
            try
            {
                foreach(var spotId in reservation.Spots)
                {
                    ChangeSpotStatus(spotId, true);
                }
            }  
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
    }

    private Spot GetSpot(Guid spotid)
    {
        return Spots.FirstOrDefault(s=>s.SpotId == spotid);
    }

    private Spot ChangeSpotStatus(Guid spotId, bool IsAvailable){
        
        var mySpot = Spots.FirstOrDefault(v=> v.SpotId == spotId);

      

        mySpot.IsAvailable = IsAvailable;

        Spots.Remove(mySpot);

        Spots.Add(mySpot);

        return mySpot;
    }
}

class Reservation {
    public Guid ReservationId { get; set; }
    public List<Guid> Spots {get;set;}
    public Vehicle VehicleType {get;set;}

    public Reservation(List<Guid> spots, Vehicle vehicleType )
    {
        Spots =  spots ?? new List<Guid>();
        VehicleType = VehicleType;

    }
}

class Spot{
    public Guid SpotId {get;set;}
    public bool IsAvailable { get; set; }
    public SpotSize Size { get; set; }
}

enum Vehicle{
    motorcyle =1 ,
    car,
    van,
}

enum SpotSize{
    small = 1,
    regular,
    large,

}


class Solution
{
    static void Main(string[] args)
    {

        var spots = new List<Spot>{
            new Spot{ SpotId =  new Guid("e474e2ec-a632-4134-bcc6-a7da887006ac")  , IsAvailable = true, Size = SpotSize.small},
            new Spot{ SpotId =  new Guid("50d07295-383a-4011-a3b1-c48b94aa8e97")  , IsAvailable = true, Size = SpotSize.small}, 
            new Spot{ SpotId =  new Guid("5986fbe6-ce3e-4a22-852b-6690ebe33b05") , IsAvailable = true, Size = SpotSize.regular}, 
            new Spot{ SpotId =  new Guid("9c434fc1-f2bc-43ee-8571-9f385028ebfd") , IsAvailable = true, Size = SpotSize.regular}, 
            new Spot{ SpotId =  new Guid("05fec531-839e-4850-92b2-d19c7a455f15") , IsAvailable = true, Size = SpotSize.regular}, 
            new Spot{ SpotId =  new Guid("9cc1ef4f-8a2f-41b5-b4dd-9c3d34dbc6ad") , IsAvailable = true, Size = SpotSize.regular}, 
            new Spot{ SpotId =  new Guid("40b1dcc1-56a0-41f1-8264-1b6d9d02a85e") , IsAvailable = true, Size = SpotSize.regular}, 
            new Spot{ SpotId =  new Guid("2b73b61a-a308-4fa2-afa0-fc3bdc06d8a4") , IsAvailable = true, Size = SpotSize.regular}, 
            new Spot{ SpotId =  new Guid("40e2bec0-ab3e-497a-8926-26203092e16d") , IsAvailable = true, Size = SpotSize.regular}, 
            new Spot{ SpotId =  new Guid("2e61abd5-5ce3-4823-9c7b-0984502c6163") , IsAvailable = true, Size = SpotSize.regular}, 
            new Spot{ SpotId =  new Guid("5feaea65-489e-4547-93a7-db285ce51ee0")  , IsAvailable = true, Size = SpotSize.large}, 
            new Spot{ SpotId =  new Guid("7ca0cc0b-ac77-4f39-aed9-2bda3d08ebcc")  , IsAvailable = true, Size = SpotSize.large}, 
            new Spot{ SpotId =  new Guid("76ba94fc-a9be-40e2-8499-d8623ce2e0b5")  , IsAvailable = true, Size = SpotSize.large}, 
            new Spot{ SpotId =  new Guid("d71bd353-6e4d-4758-aa19-c512cedc5d2b")  , IsAvailable = true, Size = SpotSize.large}, 
            new Spot{ SpotId =  new Guid("2b1207e2-9957-4de8-9484-30aafa7bf095")  , IsAvailable = true, Size = SpotSize.large}, 
            new Spot{ SpotId =  new Guid("b961b3d4-d083-4a3c-85d2-209eb3041987")  , IsAvailable = true, Size = SpotSize.large}
        };    
        
        var parking = new ParkingLot("22 ave. maindrive Los Angeles, CA",spots );

       Console.WriteLine("Location:" + parking.Location);

        PrintParkingStatus(parking);


        //Create Reservations
        //SpotId Open:5986fbe6-ce3e-4a22-852b-6690ebe33b05 | Spot size: regular
        PrintParkingAvailable(parking, SpotSize.regular);
        var luisReservation = new Reservation( new List<Guid>{{new Guid("5986fbe6-ce3e-4a22-852b-6690ebe33b05")}}, Vehicle.car);
        parking.CheckIn(luisReservation);

        
        PrintParkingStatus(parking);
        PrintParkingAvailable(parking, SpotSize.regular);

       
    }

    static void PrintParkingAvailable(ParkingLot parking , SpotSize size){
        var spotAvailable = parking.GetAvailableSpots(size);
        foreach(  var spot in spotAvailable){
            Console.WriteLine($"SpotId Open:{spot.SpotId} | Spot size: {spot.Size}");
        }
    }

    static void PrintParkingStatus(ParkingLot parking){
       //Parking IsAvailable
       Console.WriteLine($"\nTotal Spots:{parking.Spots.Count()}");
       Console.WriteLine($"Spots available:{parking.Spots.Where(v=> v.IsAvailable == true).ToList().Count}");
       Console.WriteLine($"Spots taken:{parking.Spots.Where(v=> v.IsAvailable == false).ToList().Count}");
    }
}
