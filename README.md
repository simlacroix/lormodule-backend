# LoRModule-Backend

## Name
**The Tracking Fellowship** Legends of Runeterra module backend solution.

## Description
This is the part of the application that communicates with the Riot API and returns the formatted data for the requested user to the dashboard.

## Installation
This module can be installed by running the docker-compose file containing the other modules.

    lor-module:
      image: broc1603/lor-module:latest
      ports:
        - 3700:80
      environment:
        API_KEY_LOR: <API key needed to make calls to Riot API>
      networks:
        - back-tier

    mongodb:
      image: mongo:latest
      container_name: mongodb
      restart: always
      environment:
          MONGO_INITDB_ROOT_USERNAME: admin
          MONGO_INITDB_ROOT_PASSWORD: <password>
          MONGO_INITDB_DATABASE: TheTrackingFellowship
      ports:
        - 27017:27017
      volumes:
        - mongo-data:/data/db
        - ./mongo-init.js:/docker-entrypoint-initdb.d/mongo-init.js:ro
      networks:
        - back-tier

A mongoDB databse needs to be running with the following setup script:
    
    db.createUser(
        {        
            user: "admin",        
            pwd: "<password>",
            roles: [
                {
                    role: "readWrite",
                    db: "TheTrackingFellowship"
                }
            ]
        }
    );
    db.createCollection("PlayerAccount")
    db.createCollection("TFTMatches")
    db.createCollection("LoLMatches")
    db.createCollection("LoRMatches")

### Environment Variables
* MONGODB_CONNECTION_STRING : mongodb://{_username_}:{_password_}@localhost:27017/?authSource={_username_}

## Usage
The purpose of this solution is to communicate with the Riot Games Developper API, to have access to the data on users and matches for the Legends of Runeterra video game. The module two endpoints, one to fetch data from a user and another to verify if a certain username exists. This module will take care of interpreting all the raw data given by the API by calculating some useful and meaningful data for users. This module will not be accessed by the user, it will receive requests from the Dashboard and will respond with the organized data that was requested.

| Type  | Controller | Route                | Input Field           | Response Model            |     
|-------|------------|----------------------|-----------------------|---------------------------|
| GET   | LoR        | getStatsForPlayer    | summonerName (string) | [BasicStats](#basicstats) |
| GET   | LoR        | userExists           | summonerName (string) | userExists (string)       |


### Response

#### BasicStats
    
    public double totalMatches { get; set; }
    public double wins { get; set; }
    public double losses { get; set; }
    public int? rank { get; set; }
    public double? points { get; set; }
    public List<MatchResponse?> matchHistory { get; set; }


| Name          | Type              |
|---------------|-------------------|
| summoner      | [SummonerResponse](https://developer.riotgames.com/apis#summoner-v4/GET_getByRSOPUUID:~:text=Return%20value%3A%20SummonerDTO-,SummonerDTO,-%2D%20represents%20a%20summoner)  |
| playerName    | string            |
| profileIconId | int               |
| winRatio      | double            |
| totalMatches  | double            |
| wins          | double            |
| losses        | double            |
| rank          | int               |
| points        | double            |
| matchHistory  | List<[MatchResponse](https://developer.riotgames.com/apis#lor-match-v1/GET_getMatch:~:text=Return%20value%3A%20MatchDto-,MatchDto,-NAME)> |

## Roadmap
There is no specific Roadmap past the initial scope of the project plan established for our Final Degree Project.  

The project and its structure is open for additional game module, allowing further development for after Degree project or for other team final degree project.

## Authors and acknowledgment
### Authors
* Catherine Bronsard
* David Goulet-Paradis
* Simon Lacroix
* Antoine Toutant
### Acknowledgment
* Mikaël Fortin, Project Supervisor 

## License
For open source projects, say how it is licensed.

## Project status
**In development**
