// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  backend: {
    baseURL: 'https://localhost:44351/api'
  },
  firebase: {
    apiKey: "AIzaSyAQIM70EYbErZyk31Kw051NsLbTfY03LiI",
    authDomain: "quizplay-eq4.firebaseapp.com",
    projectId: "quizplay-eq4",
    storageBucket: "quizplay-eq4.appspot.com",
    messagingSenderId: "453297139558",
    appId: "1:453297139558:web:b282967a374748cd11a2a7"
  }

};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
