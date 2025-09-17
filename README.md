# Koninkrijk
This project contains the source for the Koninkrijk game. It's a spin-off on Wordle combining it with capture-the-flag components placed on a world map of the Netherlands. 

It contains a back-end API written with C# using the asp.net core web framework. The front-end is written with React. 
It's not the best designed API, and could be decoupled more with services but it works fine for now. I would do a lot more different another time around but it was good to learn! 
Some tests are included for the back-end, which will the test the words that are used for guessing. 

The front-end was my first endeavour into React and I hated it. Some components may or may not have been yolo'd into an LLM at some point due to frustration. I'm not proud of it.  

## Running the project
Dockerfiles and some configuration for NGINX for the front-end are included and main entry points in the Program.cs and some constants should be self-explainable. 

The SQLite database can be created from the Server project with Entity Framework
```bash
cd .\koninkrijk.Server
dotnet ef database update
````

Grep for the "test.example" in the solution and replace with your own domains or IP before running. 

## Screenshots
<img width="1442" height="716" alt="image" src="https://github.com/user-attachments/assets/7fbccc6e-b2c1-4c75-bd8e-a211f024dc01" />

<img width="1437" height="875" alt="image" src="https://github.com/user-attachments/assets/32d5cff6-0aa1-44f4-b661-488039a12a2e" />



