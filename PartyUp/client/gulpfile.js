var gulp = require('gulp');
var jshint = require('gulp-jshint');
var jscs = require('gulp-jscs');
var util = require('gulp-util');

gulp.task('vet', function () {
    log('Analyzing source with JSHint and JSCS');
    return gulp.src([
        './app/**/*.js',
        './*.js'
    ])
    .pipe(jscs())
    .pipe(jshint())
    .pipe(jshint.reporter('jshint-stylish', { verbose: true }))
    .pipe(jshint.reporter('fail'));
});

////////// LOG
function log(msg) {
    if (typeof (msg) === 'object') {
        for (var item in msg) {
            if (msg.hasOwnProperty(item)) {
                util.log(util.colors.green(msg[item]));
            }
        }
    } else {
        util.log(util.colors.green(msg));
    }
}