# ⚔️ Shadow Crawlers: Tactical Dungeons ⚔️



Shadow Crawlers is an advanced online multiplayer 2D dungeon crawler game developed inside Unity. Players can seamlessly connect, create or join tactical lobbies, and complete high-stakes dungeon runs under a shared, fully synchronized network game state.



### 🚀 Key Features



**🌐 Robust Multiplayer Lobbies:** Fully integrated with Photon PUN2, supporting dynamic custom room codes, live server connection tracking, and state-of-the-art network interpolation for ultra-smooth player positioning.



**🗺️ Procedural Map Logic:** Features an automatic 2D level generator that dynamically paints customized tilemaps, obstacles, and objective layouts for every new game session.



**🕹️ Core Mechanics \& State Sync:** Features a network-ready 2D player controller with fluid grid movement, instant networked coin pickup states, and integrated enemy tracking.



**🧠 Autonomous Enemy AI:** Dungeons are populated by interactive AI patrols that automatically calculate pathfinding constraints to challenge players.



**🖥️ Optimized UI \& Subsystems:** Equipped with responsive connection screens, interactive graphics settings, tab menus, and real-time networked chat systems.



**🧼 Sanitized Codebase:** Thoroughly refactored and fully optimized for academic and production review—free of redundant scripts or localized legacy systems.



### 🛠️ Tech Stack



**🎮 Game Engine:** Unity 6 LTS



**💻 Programming Language:** C# (.NET Core)



**📡 Networking Pipeline:** Photon Unity Networking (PUN2)



**🎨 UI Interface:** TextMeshPro \& Unity UI Engine



### 📂 Architecture \& Scenes



The codebase follows strict structural separation to ensure maximum stability:



**🏰 Menu:** Powers network matchmaking, global server connection management, private room creation, and system configuration dropdowns.



**🎮 GameScene:** Hosts active multiplayer matches, live procedural tilemap generation, entity interpolation, and centralized UI status displays.



### ⚙️ Quick Setup \& Execution



Download or clone this project repository into your local machine.



Launch the folder using Unity 6.



Set your custom Photon credentials via the Unity Inspector at:

**Assets/Photon/PhotonUnityNetworking/Resources/PhotonServerSettings.asset**



Enter your unique **AppIdRealtime** into the appropriate field.



Open, load, and run the project directly from: **Assets/Scenes/Menu.unity**.



### 👤 Main Developer



Designed, refactored, and deployed by:



**⭐ Asma Taamallah ⭐**

