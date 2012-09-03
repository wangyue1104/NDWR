// Provide a default path to dwr.engine
if (typeof this['dwr'] == 'undefined') this.dwr = {};
if (typeof dwr['engine'] == 'undefined') dwr.engine = {};
if (typeof dwr.engine['_mappedClasses'] == 'undefined') dwr.engine._mappedClasses = {};

if (window['dojo']) dojo.provide('dwr.interface.demo');

if (typeof this['demo'] == 'undefined') demo = {};

demo._path = '/DWR_Test/dwr';

/**
 * @param {function|Object} callback callback function or options object
 */
demo.getDemochr = function(callback) {
  return dwr.engine._execute(demo._path, 'demo', 'getDemochr', arguments);
};

/**
 * @param {class java.lang.String} p0 a param
 * @param {function|Object} callback callback function or options object
 */
demo.setDemochr = function(p0, callback) {
  return dwr.engine._execute(demo._path, 'demo', 'setDemochr', arguments);
};


