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
    - UniqueId, Title, description, createdBy, round time, date expiration game, users associate to the Game and Card List Values.

# Add user an existing game

Given
- An anonymous user want to join in an existing Game.

When
- An anonymous user request to be added to new Game by Game unique Guid.

Then
- The Game is returned with the following details:
    - UniqueId, Title, description, createdBy, round time, date expiration game, user associate to the Game included him and Card List Values.

# A user add a Vote in Planing

Given
- An anonymous user into an existing Game.

When
- An anonymous user send a vote.

Then
- The Vote is registered for that user.


# All users in the same Game can watch all votes

Given
- An unknown number of users in the same game.
- All votes registered for all users.

When
- An anonymous user click on send Votes button.

Then
- Votes from all Users are shown in Player Panel.
