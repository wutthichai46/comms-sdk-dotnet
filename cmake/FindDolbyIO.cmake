if (DEFINED ENV{DOLBYIO_LIBRARY_PATH})
  set(DOLBYIO_LIBRARY_PATH $ENV{DOLBYIO_LIBRARY_PATH})
  message("DOLBYIO_LIBRARY_PATH = '${DOLBYIO_LIBRARY_PATH}' from environment variable")
endif()

set(DOLBYIO_SEARCH_PATHS
  /usr/local
)

if(WIN32)
  set(DOLBYIO_LIBRARY_SUFFIXES lib)
elseif(APPLE)
  set(DOLBYIO_LIBRARY_SUFFIXES "lib")
endif()

if (NOT DOLBYIO_LIBRARY_SUFFIXES)
  message(error "Platform not supported at the moment !")
endif()

find_path(DOLBYIO_INCLUDE_DIR
  HINTS
    ${DOLBYIO_LIBRARY_PATH}
  NAMES
    "dolbyio/comms/sdk.h"
  PATH_SUFFIXES
    "include/" include
  PATHS
    ${DOLBYIO_SEARCH_PATHS}
)

set(DOLBYIO_BINARY_DIR ${DOLBYIO_INCLUDE_DIR}/../bin)

find_path(DOLBYIO_IAPI_TEST_INCLUDE_DIR
  HINTS
    ${DOLBYIO_LIBRARY_PATH}
  NAMES
    "test_api/test_api.h"
  PATH_SUFFIXES
    "include/iapi" include
  PATHS
    ${DOLBYIO_SEARCH_PATHS}
)

find_library(DOLBYIO_LIBRARY_SDK
  HINTS
    ${DOLBYIO_LIBRARY_PATH}
  NAMES
    dolbyio_comms_sdk
  PATH_SUFFIXES
    ${DOLBYIO_LIBRARY_SUFFIXES}
  PATHS
    ${DOLBYIO_SEARCH_PATHS}
)

find_library(DOLBYIO_LIBRARY_MEDIA
  HINTS
    ${DOLBYIO_LIBRARY_PATH}
  NAMES
    dolbyio_comms_media
  PATH_SUFFIXES
    ${DOLBYIO_LIBRARY_SUFFIXES}
  PATHS
    ${DOLBYIO_SEARCH_PATHS}
)

find_library(DOLBYIO_LIBRARY_IAPI_TEST
  HINTS
    ${DOLBYIO_LIBRARY_PATH}
  NAMES
    iapi_test
  PATH_SUFFIXES
    ${DOLBYIO_LIBRARY_SUFFIXES}
  PATHS
    ${DOLBYIO_SEARCH_PATHS}
)

find_library(DOLBYIO_LIBRARY_DVC
  HINTS
    ${DOLBYIO_LIBRARY_PATH}
  NAMES
    dvclient
  PATH_SUFFIXES
    ${DOLBYIO_LIBRARY_SUFFIXES}
  PATHS
    ${DOLBYIO_SEARCH_PATHS}
)

find_library(DOLBYIO_LIBRARY_DNR
  HINTS
    ${DOLBYIO_LIBRARY_PATH}
  NAMES
    dvdnr
  PATH_SUFFIXES
    ${DOLBYIO_LIBRARY_SUFFIXES}
  PATHS
    ${DOLBYIO_SEARCH_PATHS}
)

if (WIN32)
  find_file(DOLBYIO_LIBRARY_SDK_IMPORTED
    HINTS
      ${DOLBYIO_LIBRARY_PATH}
    NAMES
      dolbyio_comms_sdk.dll
    PATH_SUFFIXES
      bin
    PATHS
      ${DOLBYIO_SEARCH_PATHS}
  )

  find_file(DOLBYIO_LIBRARY_MEDIA_IMPORTED
    HINTS
      ${DOLBYIO_LIBRARY_PATH}
    NAMES
      dolbyio_comms_media.dll
    PATH_SUFFIXES
      bin
    PATHS
      ${DOLBYIO_SEARCH_PATHS}
  )

  find_file(DOLBYIO_LIBRARY_DVC_IMPORTED
    HINTS
      ${DOLBYIO_LIBRARY_PATH}
    NAMES
      dvclient.dll
    PATH_SUFFIXES
      bin
    PATHS
      ${DOLBYIO_SEARCH_PATHS}
  )

  find_file(DOLBYIO_LIBRARY_DNR_IMPORTED
  HINTS
    ${DOLBYIO_LIBRARY_PATH}
  NAMES
    dvdnr.dll
  PATH_SUFFIXES
    bin
  PATHS
    ${DOLBYIO_SEARCH_PATHS}
  )

  find_file(DOLBYIO_LIBRARY_IAPI_TEST_IMPORTED
    HINTS
      ${DOLBYIO_LIBRARY_PATH}
    NAMES
      iapi_test.dll
    PATH_SUFFIXES
      bin
    PATHS
      ${DOLBYIO_SEARCH_PATHS}
  )

  add_library(avcodec SHARED IMPORTED)
  set_target_properties(avcodec PROPERTIES
    IMPORTED_IMPLIB ""
    IMPORTED_LOCATION ${DOLBYIO_BINARY_DIR}/avcodec-58.dll
  )

  add_library(avformat SHARED IMPORTED)
  set_target_properties(avformat PROPERTIES
    IMPORTED_IMPLIB ""
    IMPORTED_LOCATION ${DOLBYIO_BINARY_DIR}/avformat-58.dll
  )

  add_library(avutil SHARED IMPORTED)
  set_target_properties(avutil PROPERTIES
    IMPORTED_IMPLIB ""
    IMPORTED_LOCATION ${DOLBYIO_BINARY_DIR}/avutil-56.dll
  )
else()
  set(DOLBYIO_LIBRARY_SDK_IMPORTED ${DOLBYIO_LIBRARY_SDK})
  set(DOLBYIO_LIBRARY_MEDIA_IMPORTED ${DOLBYIO_LIBRARY_MEDIA})
  set(DOLBYIO_LIBRARY_IAPI_TEST_IMPORTED ${DOLBYIO_LIBRARY_IAPI_TEST})
  set(DOLBYIO_LIBRARY_DVC_IMPORTED ${DOLBYIO_LIBRARY_DVC})
  set(DOLBYIO_LIBRARY_DNR_IMPORTED ${DOLBYIO_LIBRARY_DNR})
endif()


add_library(DolbyioComms::sdk SHARED IMPORTED)
set_target_properties(DolbyioComms::sdk PROPERTIES
  IMPORTED_IMPLIB ${DOLBYIO_LIBRARY_SDK}
  IMPORTED_LOCATION ${DOLBYIO_LIBRARY_SDK_IMPORTED}
  INTERFACE_INCLUDE_DIRECTORIES ${DOLBYIO_INCLUDE_DIR}
  LINKER_LANGUAGE CXX
)

add_library(DolbyioComms::media SHARED IMPORTED)
set_target_properties(DolbyioComms::media PROPERTIES
  IMPORTED_IMPLIB ${DOLBYIO_LIBRARY_MEDIA}
  IMPORTED_LOCATION ${DOLBYIO_LIBRARY_MEDIA_IMPORTED}
  INTERFACE_INCLUDE_DIRECTORIES ${DOLBYIO_INCLUDE_DIR}
  LINKER_LANGUAGE CXX
)

add_library(dvc SHARED IMPORTED)
set_target_properties(dvc PROPERTIES
  IMPORTED_IMPLIB ${DOLBYIO_LIBRARY_DVC}
  IMPORTED_LOCATION ${DOLBYIO_LIBRARY_DVC_IMPORTED}
  INTERFACE_INCLUDE_DIRECTORIES ${DOLBYIO_INCLUDE_DIR}
  LINKER_LANGUAGE CXX
)

add_library(dnr SHARED IMPORTED)
set_target_properties(dnr PROPERTIES
  IMPORTED_IMPLIB ${DOLBYIO_LIBRARY_DNR}
  IMPORTED_LOCATION ${DOLBYIO_LIBRARY_DNR_IMPORTED}
  INTERFACE_INCLUDE_DIRECTORIES ${DOLBYIO_INCLUDE_DIR}
  LINKER_LANGUAGE CXX
)

if (NOT WIN32)
  target_link_libraries(DolbyioComms::sdk INTERFACE dvc)
  target_link_libraries(DolbyioComms::sdk INTERFACE dnr)
endif()

add_library(iapi_test SHARED IMPORTED)
set_target_properties(iapi_test PROPERTIES
  IMPORTED_IMPLIB ${DOLBYIO_LIBRARY_IAPI_TEST}
  IMPORTED_LOCATION ${DOLBYIO_LIBRARY_IAPI_TEST_IMPORTED}
  INTERFACE_INCLUDE_DIRECTORIES ${DOLBYIO_IAPI_TEST_INCLUDE_DIR}
)

mark_as_advanced(
  DOLBYIO_INCLUDE_DIR
  DOLBYIO_LIBRARY_SDK
  DOLBYIO_LIBRARY_MEDIA
  DOLBYIO_LIBRARY_IAPI_TEST
)

if (DOLBYIO_INCLUDE_DIR AND DOLBYIO_LIBRARY_SDK AND DOLBYIO_LIBRARY_MEDIA)
  message("Found DolbyIO library successfully: " ${DOLBYIO_LIBRARY_PATH})
  set(DOLBYIO_FOUND 1)
else()
  set(DOLBYIO_FOUND 0)
  message(ERROR ${DOLBYIO_LIBRARY_SDK})
  message(FATAL_ERROR "DolbyIO library was not found.\n"
      "Please check 'DOLBYIO_LIBRARY_PATH'.\n")
endif()