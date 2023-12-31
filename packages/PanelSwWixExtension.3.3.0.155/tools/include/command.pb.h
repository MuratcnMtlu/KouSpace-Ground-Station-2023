// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: command.proto

#ifndef PROTOBUF_command_2eproto__INCLUDED
#define PROTOBUF_command_2eproto__INCLUDED

#include <string>

#include <google/protobuf/stubs/common.h>

#if GOOGLE_PROTOBUF_VERSION < 3005000
#error This file was generated by a newer version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please update
#error your headers.
#endif
#if 3005001 < GOOGLE_PROTOBUF_MIN_PROTOC_VERSION
#error This file was generated by an older version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please
#error regenerate this file with a newer version of protoc.
#endif

#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/arena.h>
#include <google/protobuf/arenastring.h>
#include <google/protobuf/generated_message_table_driven.h>
#include <google/protobuf/generated_message_util.h>
#include <google/protobuf/metadata_lite.h>
#include <google/protobuf/message_lite.h>
#include <google/protobuf/repeated_field.h>  // IWYU pragma: export
#include <google/protobuf/extension_set.h>  // IWYU pragma: export
// @@protoc_insertion_point(includes)

namespace protobuf_command_2eproto {
// Internal implementation detail -- do not use these members.
struct TableStruct {
  static const ::google::protobuf::internal::ParseTableField entries[];
  static const ::google::protobuf::internal::AuxillaryParseTableField aux[];
  static const ::google::protobuf::internal::ParseTable schema[1];
  static const ::google::protobuf::internal::FieldMetadata field_metadata[];
  static const ::google::protobuf::internal::SerializationTable serialization_table[];
  static const ::google::protobuf::uint32 offsets[];
};
void InitDefaultsCommandImpl();
void InitDefaultsCommand();
inline void InitDefaults() {
  InitDefaultsCommand();
}
}  // namespace protobuf_command_2eproto
namespace com {
namespace panelsw {
namespace ca {
class Command;
class CommandDefaultTypeInternal;
extern CommandDefaultTypeInternal _Command_default_instance_;
}  // namespace ca
}  // namespace panelsw
}  // namespace com
namespace com {
namespace panelsw {
namespace ca {

// ===================================================================

class Command : public ::google::protobuf::MessageLite /* @@protoc_insertion_point(class_definition:com.panelsw.ca.Command) */ {
 public:
  Command();
  virtual ~Command();

  Command(const Command& from);

  inline Command& operator=(const Command& from) {
    CopyFrom(from);
    return *this;
  }
  #if LANG_CXX11
  Command(Command&& from) noexcept
    : Command() {
    *this = ::std::move(from);
  }

  inline Command& operator=(Command&& from) noexcept {
    if (GetArenaNoVirtual() == from.GetArenaNoVirtual()) {
      if (this != &from) InternalSwap(&from);
    } else {
      CopyFrom(from);
    }
    return *this;
  }
  #endif
  static const Command& default_instance();

  static void InitAsDefaultInstance();  // FOR INTERNAL USE ONLY
  static inline const Command* internal_default_instance() {
    return reinterpret_cast<const Command*>(
               &_Command_default_instance_);
  }
  static PROTOBUF_CONSTEXPR int const kIndexInFileMessages =
    0;

  void Swap(Command* other);
  friend void swap(Command& a, Command& b) {
    a.Swap(&b);
  }

  // implements Message ----------------------------------------------

  inline Command* New() const PROTOBUF_FINAL { return New(NULL); }

  Command* New(::google::protobuf::Arena* arena) const PROTOBUF_FINAL;
  void CheckTypeAndMergeFrom(const ::google::protobuf::MessageLite& from)
    PROTOBUF_FINAL;
  void CopyFrom(const Command& from);
  void MergeFrom(const Command& from);
  void Clear() PROTOBUF_FINAL;
  bool IsInitialized() const PROTOBUF_FINAL;

  size_t ByteSizeLong() const PROTOBUF_FINAL;
  bool MergePartialFromCodedStream(
      ::google::protobuf::io::CodedInputStream* input) PROTOBUF_FINAL;
  void SerializeWithCachedSizes(
      ::google::protobuf::io::CodedOutputStream* output) const PROTOBUF_FINAL;
  void DiscardUnknownFields();
  int GetCachedSize() const PROTOBUF_FINAL { return _cached_size_; }
  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const;
  void InternalSwap(Command* other);
  private:
  inline ::google::protobuf::Arena* GetArenaNoVirtual() const {
    return NULL;
  }
  inline void* MaybeArenaPtr() const {
    return NULL;
  }
  public:

  ::std::string GetTypeName() const PROTOBUF_FINAL;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  // string handler = 1;
  void clear_handler();
  static const int kHandlerFieldNumber = 1;
  const ::std::string& handler() const;
  void set_handler(const ::std::string& value);
  #if LANG_CXX11
  void set_handler(::std::string&& value);
  #endif
  void set_handler(const char* value);
  void set_handler(const char* value, size_t size);
  ::std::string* mutable_handler();
  ::std::string* release_handler();
  void set_allocated_handler(::std::string* handler);

  // bytes details = 3;
  void clear_details();
  static const int kDetailsFieldNumber = 3;
  const ::std::string& details() const;
  void set_details(const ::std::string& value);
  #if LANG_CXX11
  void set_details(::std::string&& value);
  #endif
  void set_details(const char* value);
  void set_details(const void* value, size_t size);
  ::std::string* mutable_details();
  ::std::string* release_details();
  void set_allocated_details(::std::string* details);

  // uint32 cost = 2;
  void clear_cost();
  static const int kCostFieldNumber = 2;
  ::google::protobuf::uint32 cost() const;
  void set_cost(::google::protobuf::uint32 value);

  // @@protoc_insertion_point(class_scope:com.panelsw.ca.Command)
 private:

  ::google::protobuf::internal::InternalMetadataWithArenaLite _internal_metadata_;
  ::google::protobuf::internal::ArenaStringPtr handler_;
  ::google::protobuf::internal::ArenaStringPtr details_;
  ::google::protobuf::uint32 cost_;
  mutable int _cached_size_;
  friend struct ::protobuf_command_2eproto::TableStruct;
  friend void ::protobuf_command_2eproto::InitDefaultsCommandImpl();
};
// ===================================================================


// ===================================================================

#ifdef __GNUC__
  #pragma GCC diagnostic push
  #pragma GCC diagnostic ignored "-Wstrict-aliasing"
#endif  // __GNUC__
// Command

// string handler = 1;
inline void Command::clear_handler() {
  handler_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline const ::std::string& Command::handler() const {
  // @@protoc_insertion_point(field_get:com.panelsw.ca.Command.handler)
  return handler_.GetNoArena();
}
inline void Command::set_handler(const ::std::string& value) {
  
  handler_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), value);
  // @@protoc_insertion_point(field_set:com.panelsw.ca.Command.handler)
}
#if LANG_CXX11
inline void Command::set_handler(::std::string&& value) {
  
  handler_.SetNoArena(
    &::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::move(value));
  // @@protoc_insertion_point(field_set_rvalue:com.panelsw.ca.Command.handler)
}
#endif
inline void Command::set_handler(const char* value) {
  GOOGLE_DCHECK(value != NULL);
  
  handler_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::string(value));
  // @@protoc_insertion_point(field_set_char:com.panelsw.ca.Command.handler)
}
inline void Command::set_handler(const char* value, size_t size) {
  
  handler_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(),
      ::std::string(reinterpret_cast<const char*>(value), size));
  // @@protoc_insertion_point(field_set_pointer:com.panelsw.ca.Command.handler)
}
inline ::std::string* Command::mutable_handler() {
  
  // @@protoc_insertion_point(field_mutable:com.panelsw.ca.Command.handler)
  return handler_.MutableNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline ::std::string* Command::release_handler() {
  // @@protoc_insertion_point(field_release:com.panelsw.ca.Command.handler)
  
  return handler_.ReleaseNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline void Command::set_allocated_handler(::std::string* handler) {
  if (handler != NULL) {
    
  } else {
    
  }
  handler_.SetAllocatedNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), handler);
  // @@protoc_insertion_point(field_set_allocated:com.panelsw.ca.Command.handler)
}

// uint32 cost = 2;
inline void Command::clear_cost() {
  cost_ = 0u;
}
inline ::google::protobuf::uint32 Command::cost() const {
  // @@protoc_insertion_point(field_get:com.panelsw.ca.Command.cost)
  return cost_;
}
inline void Command::set_cost(::google::protobuf::uint32 value) {
  
  cost_ = value;
  // @@protoc_insertion_point(field_set:com.panelsw.ca.Command.cost)
}

// bytes details = 3;
inline void Command::clear_details() {
  details_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline const ::std::string& Command::details() const {
  // @@protoc_insertion_point(field_get:com.panelsw.ca.Command.details)
  return details_.GetNoArena();
}
inline void Command::set_details(const ::std::string& value) {
  
  details_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), value);
  // @@protoc_insertion_point(field_set:com.panelsw.ca.Command.details)
}
#if LANG_CXX11
inline void Command::set_details(::std::string&& value) {
  
  details_.SetNoArena(
    &::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::move(value));
  // @@protoc_insertion_point(field_set_rvalue:com.panelsw.ca.Command.details)
}
#endif
inline void Command::set_details(const char* value) {
  GOOGLE_DCHECK(value != NULL);
  
  details_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::string(value));
  // @@protoc_insertion_point(field_set_char:com.panelsw.ca.Command.details)
}
inline void Command::set_details(const void* value, size_t size) {
  
  details_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(),
      ::std::string(reinterpret_cast<const char*>(value), size));
  // @@protoc_insertion_point(field_set_pointer:com.panelsw.ca.Command.details)
}
inline ::std::string* Command::mutable_details() {
  
  // @@protoc_insertion_point(field_mutable:com.panelsw.ca.Command.details)
  return details_.MutableNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline ::std::string* Command::release_details() {
  // @@protoc_insertion_point(field_release:com.panelsw.ca.Command.details)
  
  return details_.ReleaseNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline void Command::set_allocated_details(::std::string* details) {
  if (details != NULL) {
    
  } else {
    
  }
  details_.SetAllocatedNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), details);
  // @@protoc_insertion_point(field_set_allocated:com.panelsw.ca.Command.details)
}

#ifdef __GNUC__
  #pragma GCC diagnostic pop
#endif  // __GNUC__

// @@protoc_insertion_point(namespace_scope)

}  // namespace ca
}  // namespace panelsw
}  // namespace com

// @@protoc_insertion_point(global_scope)

#endif  // PROTOBUF_command_2eproto__INCLUDED
