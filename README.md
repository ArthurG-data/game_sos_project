# **Game Application with Advanced Design Patterns** 🎮

![Language](https://img.shields.io/badge/language-C%23-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-Completed-success)

## 🌟 **Project Overview**

This C# project implements a fully-featured game system with the following capabilities:
- Play modes: **Human vs. Human**, **Computer vs. Human**, and **Computer vs. Computer**.
- Save and load functionality to resume games with complete player, board, and score reconstruction.
- Undo functionality with limitations on AI moves.
- Dynamic board shapes and sizes.
- Adherence to **Object-Oriented Programming** principles and **SOLID** design patterns.

### SOS Game Rules:

The game is played on a grid-based board where players take turns placing either an "S" or an "O".
The objective is to form the sequence "SOS" in any direction: horizontally, vertically, or diagonally.
Players score a point each time they complete an "SOS".
The game continues until the board is full, and the player with the most points wins.

---

## 📖 **Table of Contents**

- [Features](#features)
- [Class Diagram](#class-diagram)
- [Sequence Diagram](#sequence-diagram)
- [Object Diagram](#object-diagram)
- [Installation](#installation)
- [Usage](#usage)
- [Design Principles and Patterns](#design-principles-and-patterns)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgments](#acknowledgments)

---

## 🌟 **Features**

1. Multiple play modes with AI integration.
2. Move validation and scoring mechanisms.
3. Game progress is saved in a text file.
4. Instructions and rules displayed before the game starts.
5. Support for different board types (rectangular, circular, etc.).
6. Implements a command pattern for undo/redo functionality.

---

## 📐 **Class Diagram**

Include the class diagram image here for a detailed architectural overview:

![Class Diagram](path_to_class_diagram.png)

---

## 📜 **Sequence Diagram**

The sequence diagram shows how the game initializes, sets up players, and begins the main loop:

![Sequence Diagram](path_to_sequence_diagram.png)

---

## 🖼️ **Object Diagram**

The object diagram provides a snapshot of the game's runtime structure:

![Object Diagram](path_to_object_diagram.png)

---

## 🔧 **Installation**

1. Clone the repository:
   ```bash
   git clone https://github.com/your_username/your_repo.git
   cd your_repo
   ```
2. Build the project:
   ```bash
   dotnet build
   ```
3. Run the game:
   ```bash
   dotnet run
   ```

---

## 🚀 **Usage**

1. Launch the application:
   - Open a terminal, navigate to the project directory, and use `dotnet run`.
   - Alternatively, use Visual Studio to build and run the project.
2. Navigate menus using arrow keys:
   - Choose a game type (e.g., Human vs. AI).
   - Set board size, player names, and other configurations.
3. Play the game:
   - Follow on-screen instructions to make moves.
   - Save the game by entering `-2`, and exit with `-3`.

---

## 💡 **Design Principles and Patterns**

This project heavily relies on advanced design principles and patterns:

- **Command Pattern**: Handles move execution and undo functionality.
- **Decorator Pattern**: Dynamically adds features to tiles (e.g., pieces or numbers).
- **Strategy Pattern**: Supports multiple move strategies (e.g., AI vs. Human).
- **Composite Pattern**: Simplifies menu management.
- **Template Method Pattern**: Defines a general sequence of operations for different games.

**SOLID Principles:**
- **Single Responsibility**: Each class focuses on one responsibility (e.g., `GameLoader` handles saving/loading).
- **Dependency Inversion**: Interfaces like `IMoveStrategy` and `IGameLogic` promote flexibility.
- **Open-Closed Principle**: New features can be added without modifying existing code.

---

## 🤝 **Contributing**

Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a feature branch:
   ```bash
   git checkout -b feature-name
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add a new feature"
   ```
4. Push to your branch and open a pull request.

---

## 📄 **License**

Distributed under the MIT License. See `LICENSE` for more information.

---

## 🙌 **Acknowledgments**

This project was independently developed, incorporating ideas and inspiration from:
- **Books and resources on design patterns and SOLID principles.**
- Contributions from the broader developer community.
