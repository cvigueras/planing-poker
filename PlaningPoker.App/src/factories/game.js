export default {
    create(username, gamename, description) {
        return {
            id: '',
            createdBy: username,
            title: gamename,
            description: description,
            roundTime: 90,
            expiration: 90
        };
    }
};