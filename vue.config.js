/*=========================================================================================
  File Name: vue.config.js
  Description: configuration file of vue  
==========================================================================================*/
module.exports = {
  publicPath: '/',
  transpileDependencies: [
    'vue-echarts',
    'resize-detector'
  ],
  configureWebpack: {
    devtool: 'source-map',
    optimization: {
      splitChunks: {
        chunks: 'all'
      }
    }
  }
}