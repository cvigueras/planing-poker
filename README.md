# Create new shared game

Given
- An anonymous user.

When
- An anonymous user creates a new game.
- With the following details:
    - Title and description, createdBy, round time and date expiration game.

Then
- The game is created in the system returning a Game unique Guid.

# Get an existing game

Given
- An existing Game created.

When
- An anonymous user request the existing Game by the unique Id received.

Then
- The Game is returned with the following details:
    - UniqueId, Title, description, createdBy, round time, date expiration game and user associate to the Game.

# Add user an existing game

Given
- An anonymous user want to join in a existing Game.

When
- An anonymous user request to be added to new Game by Game unique Guid.

Then
- The Game is returned with the following details:
    - UniqueId, Title, description, createdBy, round time, date expiration game and user associate to the Game included him.
