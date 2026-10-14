const fs = require('fs')
const path = require('path')

const baseFolder =
    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
        ? `${process.env.APPDATA}/ASP.NET/https`
        : `${process.env.HOME}/.aspnet/https`;

const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : "vueapp";

if (!certificateName) {
    console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
    process.exit(-1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// The dev certificate only exists on developer machines (not in CI), so serve over HTTPS only when it is found.
const https = fs.existsSync(certFilePath) && fs.existsSync(keyFilePath)
    ? { key: fs.readFileSync(keyFilePath), cert: fs.readFileSync(certFilePath) }
    : undefined;

module.exports = {
    chainWebpack: config => {
        // Title of the page (index.html uses it for the <title> tag).
        config.plugin('html').tap(args => {
            args[0].title = 'Planing Poker'
            return args
        })
    },
    devServer: {
        https,
        proxy: {
            '^/cards': {
                target: process.env.VUE_APP_ENVIROMENT
            },
            '^/game': {
                target: process.env.VUE_APP_ENVIROMENT
            },
            '^/votes': {
                target: process.env.VUE_APP_ENVIROMENT
            },
            '^/users': {
                target: process.env.VUE_APP_ENVIROMENT
            }
        },
        port: process.env.VUE_APP_REDIRECT_PORT
    }
}